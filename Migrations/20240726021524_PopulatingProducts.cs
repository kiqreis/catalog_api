using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Products(Name, Description, Price, ImgUrl, Stock, RegistrationDate, CategoryId)" +
                                 "values('Coca cola', 'soda cola 350ml', 3.5, 'cocacola.jpg', 33, now(), 1)");
            
            migrationBuilder.Sql("insert into Products(Name, Description, Price, ImgUrl, Stock, RegistrationDate, CategoryId)" +
                                 "values('Atum lunch', 'atum lunch with mayonnaise', 4.75, 'lunch.jpg', 8, now(), 2)");
            
            migrationBuilder.Sql("insert into Products(Name, Description, Price, ImgUrl, Stock, RegistrationDate, CategoryId)" +
                                 "values('Pudim', 'milk pudim 100g', 19.9, 'pudim.jpg', 17, now(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Products");
        }
    }
}
