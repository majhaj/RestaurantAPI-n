﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI_n.Entities;

namespace RestaurantAPI_n.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) };

        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if(!allowPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy).Must(value => 
            string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"SortBy is optional or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
