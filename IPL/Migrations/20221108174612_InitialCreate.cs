using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Picture = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SurveyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Statement = table.Column<string>(type: "TEXT", nullable: true),
                    Option1 = table.Column<string>(type: "TEXT", nullable: true),
                    Option2 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Option = table.Column<int>(type: "INTEGER", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "Description", "Picture", "Title" },
                values: new object[] { 1, "What do you think about these brands that compete? Tell us your preferences", null, "Brands" });

            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "Description", "Picture", "Title" },
                values: new object[] { 2, "Try answering these questions regarding relationships", null, "Relationship" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Option1", "Option2", "Statement", "SurveyId" },
                values: new object[] { 1, "Coca-Cola", "	Pepsi", "Which one is better?", 1 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Option1", "Option2", "Statement", "SurveyId" },
                values: new object[] { 2, "Nike", "Reebok", "Which one would you get?", 1 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Option1", "Option2", "Statement", "SurveyId" },
                values: new object[] { 3, "Apple", "Samsung", "Which do you think is best?", 1 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Option1", "Option2", "Statement", "SurveyId" },
                values: new object[] { 4, "Small and intimate wedding", "Large wedding", "Which do you prefer?", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId",
                table: "Questions",
                column: "SurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
