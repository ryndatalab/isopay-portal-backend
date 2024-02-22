using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class _20240222CPLXIsopay001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_head",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_head", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currency",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_types",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    acc_number = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    currency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_head_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_account_head_account_head_id",
                        column: x => x.account_head_id,
                        principalSchema: "portal",
                        principalTable: "account_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_account_currency_currency_id",
                        column: x => x.currency_id,
                        principalSchema: "portal",
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    on_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", maxLength: 255, nullable: false),
                    acc_credit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    acc_debit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    transaction_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_account_acc_credit_id",
                        column: x => x.acc_credit_id,
                        principalSchema: "portal",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transaction_account_acc_debit_id",
                        column: x => x.acc_debit_id,
                        principalSchema: "portal",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transaction_currency_currency_id",
                        column: x => x.currency_id,
                        principalSchema: "portal",
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transaction_transaction_types_transaction_type_id",
                        column: x => x.transaction_type_id,
                        principalSchema: "portal",
                        principalTable: "transaction_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_account_head_id",
                schema: "portal",
                table: "account",
                column: "account_head_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_currency_id",
                schema: "portal",
                table: "account",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_acc_credit_id",
                schema: "portal",
                table: "transaction",
                column: "acc_credit_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_acc_debit_id",
                schema: "portal",
                table: "transaction",
                column: "acc_debit_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_currency_id",
                schema: "portal",
                table: "transaction",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_transaction_type_id",
                schema: "portal",
                table: "transaction",
                column: "transaction_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "account",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "transaction_types",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "account_head",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "currency",
                schema: "portal");
        }
    }
}
