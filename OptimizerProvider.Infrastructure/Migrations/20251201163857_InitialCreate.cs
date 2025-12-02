using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimizerProvider.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "optimizer");

            migrationBuilder.CreateTable(
                name: "optimization_requests",
                schema: "optimizer",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientLatitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    ClientLongitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimization_requests", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "providers",
                schema: "optimizer",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ServiceTypes = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 5.00m),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_providers", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "optimization_results",
                schema: "optimizer",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EtaMinutes = table.Column<int>(type: "int", nullable: false),
                    DistanceKm = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimization_results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_optimization_results_optimization_requests_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "optimizer",
                        principalTable: "optimization_requests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_optimization_results_providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "optimizer",
                        principalTable: "providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_availability",
                schema: "optimizer",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_availability", x => x.ProviderId);
                    table.ForeignKey(
                        name: "FK_provider_availability_providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "optimizer",
                        principalTable: "providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_location",
                schema: "optimizer",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_location", x => x.ProviderId);
                    table.ForeignKey(
                        name: "FK_provider_location_providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "optimizer",
                        principalTable: "providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_workload",
                schema: "optimizer",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveCases = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_workload", x => x.ProviderId);
                    table.ForeignKey(
                        name: "FK_provider_workload_providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "optimizer",
                        principalTable: "providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_optimization_results_ProviderId",
                schema: "optimizer",
                table: "optimization_results",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_optimization_results_RequestId",
                schema: "optimizer",
                table: "optimization_results",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "optimization_results",
                schema: "optimizer");

            migrationBuilder.DropTable(
                name: "provider_availability",
                schema: "optimizer");

            migrationBuilder.DropTable(
                name: "provider_location",
                schema: "optimizer");

            migrationBuilder.DropTable(
                name: "provider_workload",
                schema: "optimizer");

            migrationBuilder.DropTable(
                name: "optimization_requests",
                schema: "optimizer");

            migrationBuilder.DropTable(
                name: "providers",
                schema: "optimizer");
        }
    }
}
