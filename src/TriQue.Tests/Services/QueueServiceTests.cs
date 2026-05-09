using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriQue.Data.Database;
using TriQue.Data.Repositories;
using TriQue.Helpers;
using TriQue.Services;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class QueueServiceTests
    {
        private DatabaseHelper _dbHelper;
        private QueueService _queueService;
        private QueueRepository _queueRepo;

        // Using seeded driver1 (DriverID=1) and route 101 (QueueID=1)
        private const int TestDriverID = 1;
        private const int TestRouteID = 101;
        private const int TestQueueID = 1;

        [TestInitialize]
        public void Setup()
        {
            _dbHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(_dbHelper, AppConfig.Configuration);
            dbInitializer.Initialize();

            _queueService = new QueueService();
            _queueRepo = new QueueRepository();

            // Clean queue before each test for isolation
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID)
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up after each test
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID)
            );

            // Reset driver status back to Waiting
            _dbHelper.ExecuteNonQuery(
                "UPDATE Driver SET StatusID = 1 WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        // =============================================
        // JoinQueue() TESTS
        // =============================================

        [TestMethod]
        public void JoinQueue_ShouldReturnAlreadyInQueue_WhenDriverIsAlreadyInQueue()
        {
            // Join once successfully
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // Try to join again
            string result = _queueService.JoinQueue(TestDriverID, TestRouteID);

            Assert.AreEqual("Already in queue.", result);
        }

        [TestMethod]
        public void JoinQueue_ShouldReturnCorrectPositionString_WhenSuccessfullyJoined()
        {
            string result = _queueService.JoinQueue(TestDriverID, TestRouteID);

            // First driver to join should be position 1
            Assert.AreEqual("Joined queue. Position: #1", result);
        }

        [TestMethod]
        public void JoinQueue_ShouldUpdateDriverStatus_ToWaiting_AfterJoining()
        {
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // Check that StatusID was set to 1 (Waiting)
            var result = _dbHelper.ExecuteScalar(
                "SELECT StatusID FROM Driver WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, result, "Driver status should be 1 (Waiting) after joining queue");
        }

        [TestMethod]
        public void JoinQueue_ShouldIncrementPosition_ForEachNewEntry()
        {
            // Driver 1 joins first
            string first = _queueService.JoinQueue(1, TestRouteID);

            // Driver 2 joins second
            string second = _queueService.JoinQueue(2, TestRouteID);

            // Clean up driver 2 after
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", 2)
            );

            Assert.AreEqual("Joined queue. Position: #1", first);
            Assert.AreEqual("Joined queue. Position: #2", second);
        }

        [TestMethod]
        public void JoinQueue_ShouldAddEntryToDatabase_AfterJoining()
        {
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count, "One queue entry should exist after joining");
        }

        // =============================================
        // IsDriverInQueue() TESTS
        // =============================================

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnTrue_WhenDriverIsInQueue()
        {
            // Join queue first
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnFalse_WhenDriverIsNotInQueue()
        {
            // Queue is empty (cleaned in TestInitialize)
            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnFalse_AfterDriverIsRemoved()
        {
            // Join queue
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // Remove from queue
            _queueRepo.RemoveDriverFromQueue(TestDriverID, TestQueueID);

            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsFalse(result);
        }

        // =============================================
        // EDGE CASES
        // =============================================

        [TestMethod]
        public void JoinQueue_ShouldNotCreateDuplicateEntries_WhenJoinedTwice()
        {
            _queueService.JoinQueue(TestDriverID, TestRouteID);
            _queueService.JoinQueue(TestDriverID, TestRouteID); // second attempt

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count, "Should only have one entry even after joining twice");
        }

        [TestMethod]
        public void JoinQueue_PositionShouldStartAtOne_WhenQueueIsEmpty()
        {
            int nextPosition = _queueRepo.GetNextPosition(TestQueueID);

            Assert.AreEqual(1, nextPosition, "First position in an empty queue should be 1");
        }
    }
}