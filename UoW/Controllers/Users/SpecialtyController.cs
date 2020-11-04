﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml;
using UoW.BL.Interfaces.Users;
using UoW.Models.Contracts.Requests;
using UoW.Models.Users;

namespace UoW.Controllers
{
    [Route("specialties")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private ISpecialtyService _specialtyService;
        private IFacultyService _facultyService;
        private ILectorService _lectorService;
        private IMapper _mapper;

        public SpecialtyController(ISpecialtyService specialtyService, IMapper mapper, IFacultyService facultyService, ILectorService lectorService)
        {
            _specialtyService = specialtyService;
            _facultyService = facultyService;
            _lectorService = lectorService;
            _mapper = mapper;
        }

        private bool checkUniquness(List<Speciality> specialtiesList, Speciality specialty)
        {
            var uniqueId = true;
            var uniqueName = true;
            specialtiesList.ForEach(delegate (Speciality spec)
            {
                if (spec.Id == specialty.Id) uniqueId = false;
                if (spec.Name == specialty.Name) uniqueName = false;
            });
            if (uniqueId && uniqueName) return true;

            return false;

        }

        [HttpGet("id")]
        public IActionResult GetSpecialtyById(int specialtyId) 
        {
            var result = _specialtyService.GetSpecialtyById(specialtyId);
            if (result == null) return NotFound();

            var specialty = _mapper.Map<Speciality>(result);

            return Ok(specialty);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Speciality> specialties = new List<Speciality>();
            var result = _specialtyService.GetAll();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            result.ForEach(delegate (Speciality specialty)
            {
                var mappedSpecialty = _mapper.Map<Speciality>(specialty);
                specialties.Add(mappedSpecialty);
            });

            return Ok(specialties);
        }



        [HttpPost]
        public IActionResult CreateSpecialty([FromBody] SpecialtyRequest request)
        {
            if (request == null) return NotFound();

            var specialty = _mapper.Map<Speciality>(request);

            List<Speciality> specialtiesList = _specialtyService.GetAll();

            var facultyIdExists = _facultyService.GetFacultyById(specialty.FacultyId) != null;
            var lectorIdExists = _lectorService.GetLectorId(specialty.LectorId) != null;

            if (!checkUniquness(specialtiesList, specialty))
            {
                return Conflict("Dublicate key");
            }

            if (!facultyIdExists) return NotFound("Invalid Faculty Id");
            if (!lectorIdExists) return NotFound("Invalid Lector Id");

            var result = _specialtyService.Create(specialty);

            if (result == null) return NotFound();

            return Ok(specialty);
        }

        [HttpDelete]
        public IActionResult DeleteSpecialty(int id)
        {
            if (id <= 0) return BadRequest();

            var specialty = _specialtyService.GetSpecialtyById(id);
            if (specialty == null) return NotFound();

             _specialtyService.Delete(id);

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateSpecialty([FromBody] SpecialtyRequest request)
        {
            if (request == null) return NotFound();

            var specialty = _mapper.Map<Speciality>(request);

            var result = _specialtyService.Update(specialty);
            if (result == null) return NotFound();

            return Ok(specialty);
        }
    }
}