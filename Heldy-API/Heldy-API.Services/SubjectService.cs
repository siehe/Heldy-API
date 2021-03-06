﻿using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class SubjectService : ISubjectService
    {
        private ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<int> CreateSubjectAsync(Subject createCourseRequest)
        {
            var id = await _subjectRepository.CreateSubjectAsync(createCourseRequest);
            return id;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetSubjectsAsync();
            return subjects;
        }
    }
}
