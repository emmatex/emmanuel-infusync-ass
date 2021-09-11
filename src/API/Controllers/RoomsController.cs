using Core.Entities;
using Core.Interfaces;
using System;

namespace API.Controllers
{
    public class RoomsController : BaseController
    {
        private readonly IGenericRepository<Room> _repository;

        public RoomsController(IGenericRepository<Room> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
