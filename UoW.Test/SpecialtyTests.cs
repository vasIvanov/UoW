﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoW.BL.Interfaces.Users;
using UoW.BL.Services.Users;
using UoW.Controllers;
using UoW.DL.Interfaces.Users;
using UoW.Models.Common;
using UoW.Models.Contracts.Requests;
using UoW.Models.Contracts.Responses;
using UoW.Models.Users;
using Xunit;

namespace UoW.Test
{
    public class SpecialtyTests
    {
        private IMapper _mapper;
        private Mock<ISpecialityRepository> _specialtyRepository;
        private Mock<ILectorRepository> _lectorRepository;
        private Mock<IFacultyRepository> _facultyRepository;
        private ISpecialtyService _specialtyService;
        private SpecialtyController _controller;
        IList<Specialty> _specialities = new List<Specialty>()
        {
            {new Specialty {FacultyId = 1, Id = 1, LectorId = 23, Name = "test", ShortName = "ts" } }
        };
        public SpecialtyTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            _mapper = mockMapper.CreateMapper();
            _specialtyRepository = new Mock<ISpecialityRepository>();
            _lectorRepository = new Mock<ILectorRepository>();
            _facultyRepository = new Mock<IFacultyRepository>();
            _specialtyService = new SpecialtyService(_specialtyRepository.Object, _facultyRepository.Object, _lectorRepository.Object);
            _controller = new SpecialtyController(_specialtyService, _mapper);

        }

        [Fact]
        public async Task Specialty_GetAll_Count_Check()
        {
            //setup
            var expectedCount = 1;

            _specialtyRepository.Setup(x => x.GetAll())
                .ReturnsAsync(_specialities);
            //inject

            //Act
            var result = await _controller.GetAll();

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var specialties = okObjectResult.Value as IEnumerable<SpecialtyResponse>;
            Assert.NotNull(specialties);
            Assert.Equal(expectedCount, specialties.Count());
        }

        [Fact]
        public async Task Specialty_GetById_Name_Check()
        {
            var expectedName = "test";
            var specialtyId = 1;

            _specialtyRepository.Setup(x => x.GetById(specialtyId))
                .ReturnsAsync(_specialities.FirstOrDefault(x => x.Id == specialtyId));
            var result = await _controller.GetSpecialtyById(specialtyId);
            var okObjectResult = result as OkObjectResult;
            var specialty = okObjectResult.Value as Specialty;
            Assert.NotNull(specialty);
            Assert.Equal(expectedName, specialty.Name);
        }

        [Fact]
        public async Task Specialty_GetById_NotFound()
        {
            var specialtyId = 3;

            _specialtyRepository.Setup(x => x.GetById(specialtyId))
                .ReturnsAsync(_specialities.FirstOrDefault(x => x.Id == specialtyId));

            var result = await _controller.GetSpecialtyById(specialtyId);

            var notFoundObjectResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);
        }

        [Fact]
        public async Task Specialty_Update_Name()
        {
            var specialtyId = 1;

            var specialty = _specialities.FirstOrDefault(x => x.Id == specialtyId);
            specialty.Name = "edited name";

            _specialtyRepository.Setup(x => x.Update(specialty))
                .Returns(Task.FromResult(specialty));

            var result = await _controller.UpdateSpecialty(_mapper.Map<SpecialtyRequest>(specialty));

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var spec = okObjectResult.Value as Specialty;
            Assert.NotNull(spec);
            Assert.Equal("edited name", spec.Name);
        }
    }
}
