using System;
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

        public void JoinQueue(int driverID, int routeID)
        {
            int queueID = _queueRepo.GetQueueIdByRouteId(routeID);

            bool alreadyJoined = _queueRepo.IsDriverAlreadyInQueue(queueID, driverID);
            if (alreadyJoined)
                return;

            int position = _queueRepo.GetNextPosition(queueID);
            _queueRepo.AddQueueEntry(queueID, driverID, position);
            _driverRepo.UpdateStatus(driverID, 1); // Waiting

        }
    }


}
