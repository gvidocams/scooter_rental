using System.Linq;
using System.Collections.Generic;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Helper
{
    public static class Get
    {
        public static IScooter Scooter(string id, IList<IScooter> scooters)
        {
            var scooter = scooters.FirstOrDefault(s => s.Id == id);

            if (scooter == null)
            {
                throw new ScooterDoesNotExistsException(id);
            }

            return scooter;
        }

    }
}
