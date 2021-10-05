using Easy.Transfers.Admin.Controllers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Transfers.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected IMediator MediatorService { get; }
        protected ILogger<BaseController<T>> _logger;

        protected BaseController(
            IMediator mediatorService,
            ILogger<BaseController<T>> logger)
        {
            MediatorService = mediatorService;
            _logger = logger;
        }

        protected virtual async Task<IActionResult> GenerateResponseAsync(Func<Task> func, HttpStatusCode responseCode)
        {
            try
            {
                await func();

                return StatusCode((int)responseCode);
            }
            catch
            {
                throw;
            }
        }

        protected virtual async Task<IActionResult> GenerateResponseAsync<TDataObject>(Func<Task<TDataObject>> func)
        {
            return await GenerateResponseAsync(func, HttpStatusCode.OK);
        }

        protected virtual async Task<IActionResult> GenerateResponseAsync<TDataObject>(Func<Task<TDataObject>> func, HttpStatusCode responseCode)
        {
            try
            {
                var response = await func();
                
                return StatusCode((int)responseCode, response);
            }
            catch (ValidationException validExcep)
            {
                return HandleValidationExceptionResult(validExcep);
            }
            catch (HttpRequestException ex)
            {
                return HandleHttpExceptionResult(ex);
            }
            catch (MongoException ex)
            {
                return HandleMongoExceptionResult(ex);
            }
            catch (Exception ex)
            {
                return HandleExceptionResult(ex);
            }
        }

        private IActionResult HandleMongoExceptionResult(MongoException ex)
        {
            _logger.LogError($"Error MongoException {ex?.Message}");

            var notifications = new List<string>();

            notifications.Add(ex?.Message);
           
            return StatusCode(500, new BaseControllerResponse<object>()
            {
                Mensagens = notifications
            });
        }

        private IActionResult HandleHttpExceptionResult(HttpRequestException ex)
        {
            _logger.LogError($"Error HandleHttpException - {ex?.Message}");

            var notifications = new List<string>();

            var statusCode = (int)ex.StatusCode;

            notifications.Add(ex?.Message);

            return StatusCode(statusCode, new BaseControllerResponse<object>()
            {
                Mensagens = notifications
            });
        }

        private IActionResult HandleExceptionResult(Exception ex)
        {
            var notifications = new List<string>();
            
            notifications.Add("Ocorreu um erro Interno. Contate o administrador");

            _logger.LogError($"Error HandleException - {ex?.Message}");

            var statusCode = 500;

            return StatusCode((int)statusCode, new BaseControllerResponse<object>()
            {
                Mensagens = notifications
            });
        }

        private IActionResult HandleValidationExceptionResult(ValidationException e)
        {
            var statusCode = 400;
            var errors = e.Errors.Select(x => x.ErrorMessage).ToList();

            _logger.LogInformation($"Error  HandleValidationException - {string.Concat(errors)}");

            return StatusCode((int)statusCode, new  BaseControllerResponse<object>()
            {
                Mensagens = errors
            });
        }
    }
}