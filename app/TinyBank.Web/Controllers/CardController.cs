using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Extensions;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICardService _cardService;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;
        public CardController(TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICustomerService customerService,
            ICardService cardService)
        {
            _logger = logger;
            _customerService = customerService;
            _cardService = cardService;
            _dbContext = dbContext;

        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _cardService.GetById(id);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return View(result.Data);
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] PaymentOptions options)
        {
            var result = _cardService.Checkout(options);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Index(PaymentOptions options)
        {
            return View(
                new PaymentViewModel() { 
                    PaymentOptions = options ?? new PaymentOptions()
                });

        }



    }
}
