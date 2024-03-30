using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork) //ask CLR for Create Object from class implement Interface IEmployeeRepository
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchValue))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);


            var MappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);

        }
        public ActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                MappedEmployee.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                int result = _unitOfWork.Complete();
                if (result > 0)
                    TempData["message"] = "Employee is added";

                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }


        public IActionResult Details(int? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee == null)
                return NotFound();
            var department = _unitOfWork.DepartmentRepository.GetById((int)employee.DepartmentId);
            ViewBag.DepartmentName = department.Name;
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
        }


        public IActionResult Edit(int? id)
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
              var result=  _unitOfWork.Complete();
                if (result > 0 && employeeVM.ImageName!=null)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeVM);

        }


    }
}
