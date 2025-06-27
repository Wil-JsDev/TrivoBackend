using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivo.Infraestructura.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    PkAdministradorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Biografia = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContrasenaHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FotoPerfil = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Linkedin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKAdministradorId", x => x.PkAdministradorId);
                });

            migrationBuilder.CreateTable(
                name: "Habilidad",
                columns: table => new
                {
                    PkHabilidadId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKHabilidadId", x => x.PkHabilidadId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    PkUsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Biografia = table.Column<string>(type: "text", nullable: false),
                    CuentaConfirmada = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContrasenaHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ubicacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FotoPerfil = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Linkedin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EstadoUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKUsuarioId", x => x.PkUsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Codigo",
                columns: table => new
                {
                    PkCodigoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    Valor = table.Column<string>(type: "text", nullable: false),
                    Usado = table.Column<bool>(type: "boolean", nullable: false),
                    Expiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revocado = table.Column<bool>(type: "boolean", nullable: false),
                    RefrescarCodigo = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKCodigoId", x => x.PkCodigoId);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experto",
                columns: table => new
                {
                    PkExpertoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisponibleParaProyectos = table.Column<bool>(type: "boolean", nullable: false),
                    Contratado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKExpertoId", x => x.PkExpertoId);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    PkNotificacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(50)", nullable: false),
                    Contenido = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Leida = table.Column<bool>(type: "boolean", nullable: false),
                    FechaLeida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKNotificacionId", x => x.PkNotificacionId);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reclutador",
                columns: table => new
                {
                    PkReclutadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreEmpresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKReclutadorId", x => x.PkReclutadorId);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioHabilidad",
                columns: table => new
                {
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkHabilidadId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nivel = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioHabilidad", x => new { x.FkUsuariosId, x.FkHabilidadId });
                    table.ForeignKey(
                        name: "FKHabilidadId",
                        column: x => x.FkHabilidadId,
                        principalTable: "Habilidad",
                        principalColumn: "PkHabilidadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emparejamiento",
                columns: table => new
                {
                    PkEmparejamientoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkReclutadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkExpertoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpertoEstado = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReclutadorEstado = table.Column<string>(type: "varchar(50)", nullable: false),
                    EmparejamientoEstado = table.Column<string>(type: "varchar(50)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKEmparejamientoId", x => x.PkEmparejamientoId);
                    table.ForeignKey(
                        name: "FKExpertoId",
                        column: x => x.FkExpertoId,
                        principalTable: "Experto",
                        principalColumn: "PkExpertoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKReclutadorId",
                        column: x => x.FkReclutadorId,
                        principalTable: "Reclutador",
                        principalColumn: "PkReclutadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaInteres",
                columns: table => new
                {
                    PkCategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InteresId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKCategoriaInteresId", x => x.PkCategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Interes",
                columns: table => new
                {
                    PkInteresId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FkCategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkCreadoPorId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKInteresId", x => x.PkInteresId);
                    table.ForeignKey(
                        name: "FKCategoriaId",
                        column: x => x.FkCategoriaId,
                        principalTable: "CategoriaInteres",
                        principalColumn: "PkCategoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKCreadoPorId",
                        column: x => x.FkCreadoPorId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioInteres",
                columns: table => new
                {
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkInteresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioInteres", x => new { x.FkInteresId, x.FkUsuariosId });
                    table.ForeignKey(
                        name: "FKInteresId",
                        column: x => x.FkInteresId,
                        principalTable: "Interes",
                        principalColumn: "PkInteresId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    PkChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoChat = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PkChatId", x => x.PkChatId);
                });

            migrationBuilder.CreateTable(
                name: "ChatUsuario",
                columns: table => new
                {
                    FkChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkUsuariosId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsuario", x => new { x.FkChatId, x.FkUsuariosId });
                    table.ForeignKey(
                        name: "FKChatId",
                        column: x => x.FkChatId,
                        principalTable: "Chat",
                        principalColumn: "PkChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKUsuariosId",
                        column: x => x.FkUsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensaje",
                columns: table => new
                {
                    MensajeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PkChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkEmisorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "varchar(50)", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMensajeId", x => x.MensajeId);
                    table.ForeignKey(
                        name: "FKEmisorId",
                        column: x => x.FkEmisorId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensaje_Chat_PkChatId",
                        column: x => x.PkChatId,
                        principalTable: "Chat",
                        principalColumn: "PkChatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    PkReporteId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkReportadoPorId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkMensajeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstadoReporte = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nota = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKReporteId", x => x.PkReporteId);
                    table.ForeignKey(
                        name: "FKMensajeId",
                        column: x => x.FkMensajeId,
                        principalTable: "Mensaje",
                        principalColumn: "MensajeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKReportadoPorId",
                        column: x => x.FkReportadoPorId,
                        principalTable: "Usuario",
                        principalColumn: "PkUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQAdministradorEmail",
                table: "Administrador",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQAdministradorNombreUsuario",
                table: "Administrador",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaInteres_InteresId",
                table: "CategoriaInteres",
                column: "InteresId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ChatId",
                table: "Chat",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsuario_FkUsuariosId",
                table: "ChatUsuario",
                column: "FkUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Codigo_FkUsuariosId",
                table: "Codigo",
                column: "FkUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Emparejamiento_FkExpertoId",
                table: "Emparejamiento",
                column: "FkExpertoId");

            migrationBuilder.CreateIndex(
                name: "IX_Emparejamiento_FkReclutadorId",
                table: "Emparejamiento",
                column: "FkReclutadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Experto_FkUsuariosId",
                table: "Experto",
                column: "FkUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Interes_FkCategoriaId",
                table: "Interes",
                column: "FkCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Interes_FkCreadoPorId",
                table: "Interes",
                column: "FkCreadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_FkEmisorId",
                table: "Mensaje",
                column: "FkEmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_PkChatId",
                table: "Mensaje",
                column: "PkChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_FkUsuariosId",
                table: "Notificacion",
                column: "FkUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Reclutador_FkUsuariosId",
                table: "Reclutador",
                column: "FkUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_FkMensajeId",
                table: "Reporte",
                column: "FkMensajeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_FkReportadoPorId",
                table: "Reporte",
                column: "FkReportadoPorId");

            migrationBuilder.CreateIndex(
                name: "UQUsuariosEmail",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQUsuariosNombreUsuario",
                table: "Usuario",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioHabilidad_FkHabilidadId",
                table: "UsuarioHabilidad",
                column: "FkHabilidadId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioInteres_FkUsuariosId",
                table: "UsuarioInteres",
                column: "FkUsuariosId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaInteres_Interes_InteresId",
                table: "CategoriaInteres",
                column: "InteresId",
                principalTable: "Interes",
                principalColumn: "PkInteresId");

            migrationBuilder.AddForeignKey(
                name: "FKChatId",
                table: "Chat",
                column: "ChatId",
                principalTable: "Mensaje",
                principalColumn: "MensajeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaInteres_Interes_InteresId",
                table: "CategoriaInteres");

            migrationBuilder.DropForeignKey(
                name: "FKChatId",
                table: "Chat");

            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "ChatUsuario");

            migrationBuilder.DropTable(
                name: "Codigo");

            migrationBuilder.DropTable(
                name: "Emparejamiento");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "UsuarioHabilidad");

            migrationBuilder.DropTable(
                name: "UsuarioInteres");

            migrationBuilder.DropTable(
                name: "Experto");

            migrationBuilder.DropTable(
                name: "Reclutador");

            migrationBuilder.DropTable(
                name: "Habilidad");

            migrationBuilder.DropTable(
                name: "Interes");

            migrationBuilder.DropTable(
                name: "CategoriaInteres");

            migrationBuilder.DropTable(
                name: "Mensaje");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Chat");
        }
    }
}
