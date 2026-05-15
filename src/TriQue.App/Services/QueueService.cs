using System;
using System.Data;
using TriQue.Data.Repositories;
using TriQue.Models;


namespace TriQue.Services
{
    public class QueueService
    {
        private readonly QueueRepository _queueRepo;
        private readonly DriverRepository _driverRepo;

        public QueueService()
        {
            _queueRepo = new QueueRepository();
            _driverRepo = new DriverRepository();
        }

        public int? GetQueueIdByRouteId(int routeID)
        {
            return _queueRepo.GetQueueByRouteId(routeID)?.QueueID;
        }

        public DataTable GetQueueByGroupID(int groupID, int routeID)
        {
            return _queueRepo.GetQueueByGroupID(groupID, routeID);
        }

        public bool IsDriverInQueue(int driverID, int routeID)
        {
            var queue = _queueRepo.GetQueueByRouteId(routeID);

            if (queue == null)
                return false;

            return _queueRepo.IsDriverAlreadyInQueue(queue.QueueID, driverID);
        }


        public DataRow? GetQueueDriver(int queueID, int driverID)
        {
            return _queueRepo.GetQueueDriver(queueID, driverID);
        }

        public DataTable GetQueueDrivers(int queueID)
        {
            return _queueRepo.GetQueueDrivers(queueID);
        }

        public void RemoveDriverFromQueue(int driverID, int queueID)
        {
            _queueRepo.RemoveDriverFromQueue(driverID, queueID);
        }

        public void ReorderQueuePositions(int queueID)
        {
            _queueRepo.ReorderQueuePositions(queueID);
        }

        public void ResetQueue(int routeID, int groupID)
        {
            _queueRepo.ResetQueue(routeID, groupID);
        }

        public string JoinQueue(int driverID, int routeID)
        {
            var queue = _queueRepo.GetQueueByRouteId(routeID);

            if (queue == null)
                return "Queue not found.";

            bool alreadyJoined = _queueRepo.IsDriverAlreadyInQueue(queue.QueueID, driverID);
            if (alreadyJoined)
                return "Already in queue.";

            int position = _queueRepo.GetNextPosition(queue.QueueID);

            var entry = new QueueEntry
            {
                QueueID = queue.QueueID,
                DriverID = driverID,
                QueuePosition = position,
                JoinedAt = DateTime.Now
            };

            _queueRepo.AddQueueEntry(entry);
            _driverRepo.UpdateStatus(driverID, 1);

            var row = _queueRepo.GetQueueDriver(queue.QueueID, driverID);
            int visibleRank = int.TryParse(row?["Position"]?.ToString(), out int r) ? r : position;

            return $"Joined queue. Position: #{visibleRank}";
        }
    }


}
