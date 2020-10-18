using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FvpWebApp.Data
{
    public class CountryModelConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { CountryId = 1, Symbol = "AT", NamePL = "Austria", UE = true },
                new Country { CountryId = 2, Symbol = "BE", NamePL = "Belgia", UE = true },
                new Country { CountryId = 3, Symbol = "BG", NamePL = "Bułgaria", UE = true },
                new Country { CountryId = 4, Symbol = "CY", NamePL = "Cypr", UE = true },
                new Country { CountryId = 5, Symbol = "CZ", NamePL = "Czechy", UE = true },
                new Country { CountryId = 6, Symbol = "DE", NamePL = "Niemcy", UE = true },
                new Country { CountryId = 7, Symbol = "DK", NamePL = "Dania", UE = true },
                new Country { CountryId = 8, Symbol = "EE", NamePL = "Estonia", UE = true },
                new Country { CountryId = 9, Symbol = "EL", NamePL = "Grecja", UE = true },
                new Country { CountryId = 10, Symbol = "ES", NamePL = "Hiszpania", UE = true },
                new Country { CountryId = 11, Symbol = "FI", NamePL = "Finlandia", UE = true },
                new Country { CountryId = 12, Symbol = "FR", NamePL = "Francja", UE = true },
                new Country { CountryId = 13, Symbol = "GB", NamePL = "Wielka Brytania", UE = true },
                new Country { CountryId = 14, Symbol = "HR", NamePL = "Chorwacja", UE = true },
                new Country { CountryId = 15, Symbol = "HU", NamePL = "Węgry", UE = true },
                new Country { CountryId = 16, Symbol = "IE", NamePL = "Irlandia", UE = true },
                new Country { CountryId = 17, Symbol = "IT", NamePL = "Włochy", UE = true },
                new Country { CountryId = 18, Symbol = "LT", NamePL = "Litwa", UE = true },
                new Country { CountryId = 19, Symbol = "LU", NamePL = "Luksemburg", UE = true },
                new Country { CountryId = 20, Symbol = "LV", NamePL = "Łotwa", UE = true },
                new Country { CountryId = 21, Symbol = "MT", NamePL = "Malta", UE = true },
                new Country { CountryId = 22, Symbol = "NL", NamePL = "Holandia", UE = true },
                new Country { CountryId = 23, Symbol = "PL", NamePL = "Polska", UE = true },
                new Country { CountryId = 24, Symbol = "PT", NamePL = "Portugalia", UE = true },
                new Country { CountryId = 25, Symbol = "RO", NamePL = "Rumunia", UE = true },
                new Country { CountryId = 26, Symbol = "SE", NamePL = "Szwecja", UE = true },
                new Country { CountryId = 27, Symbol = "SI", NamePL = "Słowenia", UE = true },
                new Country { CountryId = 28, Symbol = "SK", NamePL = "Słowacja", UE = true });
        }
    }
}
