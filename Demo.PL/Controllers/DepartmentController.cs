using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR for creating object from class implement Interface IDepartmentRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var MappedDepartment = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
                int result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                    TempData["message"] = "New Department Created";

                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(ViewName, MappedDepartment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(departmentVM);
        }     
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }


            return View(departmentVM);
             
        }
    }
}
