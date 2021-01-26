using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Migrations
{
    public partial class _004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPost_CategoryPost_CategoryPostId",
                table: "CategoryPost");

            migrationBuilder.DropIndex(
                name: "IX_CategoryPost_CategoryPostId",
                table: "CategoryPost");

            migrationBuilder.DropColumn(
                name: "CategoryPostId",
                table: "CategoryPost");

            migrationBuilder.RenameColumn(
                name: "ModDate",
                table: "PostComment",
                newName: "Moderated");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "PostComment",
                newName: "ModBody");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "CategoryPost",
                newName: "PostBody");

            migrationBuilder.AddColumn<string>(
                name: "CommentBody",
                table: "PostComment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "BlogCategory",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentBody",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "BlogCategory");

            migrationBuilder.RenameColumn(
                name: "Moderated",
                table: "PostComment",
                newName: "ModDate");

            migrationBuilder.RenameColumn(
                name: "ModBody",
                table: "PostComment",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "PostBody",
                table: "CategoryPost",
                newName: "Body");

            migrationBuilder.AddColumn<int>(
                name: "CategoryPostId",
                table: "CategoryPost",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_CategoryPostId",
                table: "CategoryPost",
                column: "CategoryPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPost_CategoryPost_CategoryPostId",
                table: "CategoryPost",
                column: "CategoryPostId",
                principalTable: "CategoryPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
