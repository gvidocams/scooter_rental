using ScooterRental;
using ScooterRental.Interfaces;
using ScooterRental.Exceptions;
using FluentAssertions;
using Moq.AutoMock;
using Moq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ScooterRentalMoq.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private AutoMocker _mocker;
        private Mock<IScooterService> _scooterServiceMock;
        private IRentalCompany _rentalCompany;
        private List<IRentalDetails> _rentalDetails;
        private Scooter _scooter;

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _rentalDetails = new List<IRentalDetails>();
            _scooterServiceMock = _mocker.GetMock<IScooterService>();
            _rentalCompany = new RentalCompany("Gvido Scooters", _scooterServiceMock.Object, _rentalDetails);
            _scooter = new Scooter("1", 0.2m);
        }

        [TestMethod]
        public void StartRent_RentValidScooter_ScooterIsRented()
        {
            _scooterServiceMock
                .Setup(s => s.GetScooterById("1"))
                .Returns(_scooter);

            _rentalCompany.StartRent("1");

            _scooter.IsRented.Should().BeTrue();
            _rentalDetails.Count.Should().Be(1);
        }

        [TestMethod]
        public void StartRent_RentScooterWithInvalidId_ThrowsInvalidIdException()
        {
            _scooterServiceMock
                .Setup(s => s.GetScooterById(""))
                .Throws<InvalidIdException>();

            Action act = () => _rentalCompany.StartRent("");

            act.Should().Throw<InvalidIdException>()
                .WithMessage("Id cannot be null or empty!");
        }

        [TestMethod]
        public void StartRent_RentAlreadyRentedScooter_ThrowsScooterIsStillRentedException()
        {
            _scooter.IsRented = true;

            _scooterServiceMock
                .Setup(s => s.GetScooterById("1"))
                .Returns(_scooter);

            Action act = () => _rentalCompany.StartRent("1");

            act.Should().Throw<ScooterIsStillRentedException>()
                .WithMessage("Can't remove a scooter with id 1 because it's still being rented!");
        }

        [TestMethod]
        public void EndRent_EndRentValidScooter_ScooterNotRented()
        {
            _scooter.IsRented = true;
            _rentalDetails.Add(new RentalDetails("1", DateTime.Now, 0.2m));

            _scooterServiceMock
                .Setup(s => s.GetScooterById("1"))
                .Returns(_scooter);

            _rentalCompany.EndRent("1");

            _scooter.IsRented.Should().BeFalse();
        }

        [TestMethod]
        public void EndRent_EndsRentToANotRentedScooter_ThrowsScooterIsntRentedToEndRent()
        {
            _scooterServiceMock
                .Setup(s => s.GetScooterById("1"))
                .Returns(_scooter);

            Action act = () => _rentalCompany.EndRent("1");

            act.Should().Throw<ScooterIsntRentedToEndRentException>()
                .WithMessage("Can't end rent to this scooter with id 1 because it hasn't been rented in the first place!");
        }

        [TestMethod]
        public void EndRent_ValidScooterRentalTime2Minutes_04RentalPrice()
        {
            _scooter.IsRented = true;
            _rentalDetails.Add(new RentalDetails("1", DateTime.Now.AddMinutes(-2), 0.2m));

            _scooterServiceMock
                .Setup(s => s.GetScooterById("1"))
                .Returns(_scooter);

            _rentalCompany.EndRent("1").Should().Be(0.4m);
        }
    }
}