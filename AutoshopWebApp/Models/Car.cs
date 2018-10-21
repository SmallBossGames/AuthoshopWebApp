﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoshopWebApp.Models
{
    public class Car
    {
        public int CarId { get; set; }

        [Required]
        [Display(Name = "ID модели")]
        public int MarkAndModelID { get; set; }

        [Required]
        [Display(Name = "Цвет")]
        public string Color { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Номер двигателя")]
        public string EngineNumber { get; set; }
        
        [Required]
        [Display(Name = "Номер кузова")]
        [MaxLength(100)]
        public string BodyNumber { get; set; }

        [Display(Name = "Номер шасси")]
        [MaxLength(100)]
        public string ChassisNumber { get; set; }

        [Required]
        [Display(Name = "Дата выпуска")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Номер регистрации")]
        [MaxLength(100)]
        public string RegNumber { get; set; }


        [Display(Name = "Пробег")]
        public int Run { get; set; }

        [Display(Name = "Первоначальная стоимость")]
        [DataType(DataType.Currency)]
        public decimal? ReleasePrice { get; set; }

        [Display(Name = "Стоимость продажи")]
        [DataType(DataType.Currency)]
        public decimal? SellingPrice { get; set; }

        [Display(Name = "Стоимость покупки")]
        [DataType(DataType.Currency)]
        public decimal? BuyingPrice { get; set; }

        [Display(Name = "ID справки")]
        public ReferenceTS CarReferenceId { get; set; }

        public SaleStatus SaleStatus { get; set; }
    }

    public enum SaleStatus
    {
        Expertise,
        ForBuy,
        ForSold,
        Sold
    }
}
