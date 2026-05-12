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

        public int GetQueueIdByRouteId(int routeID)
        {
            return _queueRepo.GetQueueIdByRouteId(routeID);
        }

        public bool IsDriverInQueue(int driverID, int routeID)
        {
            int queueID = _queueRepo.GetQueueIdByRouteId(routeID);
            return _queueRepo.IsDriverAlreadyInQueue(queueID, driverID);
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

        public string JoinQueue(int driverID, int routeID)
        {
            int queueID = _queueRepo.GetQueueIdByRouteId(routeID);

            bool alreadyJoined = _queueRepo.IsDriverAlreadyInQueue(queueID, driverID);
            if (alreadyJoined)
                return "Already in queue.";

            int position = _queueRepo.GetNextPosition(queueID);
            _queueRepo.AddQueueEntry(queueID, driverID, position);
            _driverRepo.UpdateStatus(driverID, 1);

            return $"Joined queue. Position: #{position}";
        }
    }


}
