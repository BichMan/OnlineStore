// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineStore.Helpers;

namespace OnlineStore.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220402021544_AddAddressShipping")]
    partial class AddAddressShipping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OnlineStore.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FK_Cart_product")
                        .HasColumnType("int");

                    b.Property<int?>("FK_Cart_user")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FK_Cart_product");

                    b.HasIndex("FK_Cart_user");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OnlineStore.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FK_Like_Product")
                        .HasColumnType("int");

                    b.Property<int?>("FK_Like_User")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FK_Like_Product");

                    b.HasIndex("FK_Like_User");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("OnlineStore.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressShipping")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FK_Order_User")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FK_Order_User");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OnlineStore.Entities.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressShipping")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FK_OrderDetail_order")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("productName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_OrderDetail_order");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("OnlineStore.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttributesId")
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FK_Brand")
                        .HasColumnType("int");

                    b.Property<int?>("FK_Color")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("FK_Brand");

                    b.HasIndex("FK_Color");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("OnlineStore.Entities.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductAttributes");
                });

            modelBuilder.Entity("OnlineStore.Entities.ProductBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductBrands");
                });

            modelBuilder.Entity("OnlineStore.Entities.Slider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("OnlineStore.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineStore.Entities.Cart", b =>
                {
                    b.HasOne("OnlineStore.Entities.Product", "product")
                        .WithMany()
                        .HasForeignKey("FK_Cart_product");

                    b.HasOne("OnlineStore.Entities.User", "user")
                        .WithMany()
                        .HasForeignKey("FK_Cart_user");

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("OnlineStore.Entities.Like", b =>
                {
                    b.HasOne("OnlineStore.Entities.Product", "product")
                        .WithMany()
                        .HasForeignKey("FK_Like_Product");

                    b.HasOne("OnlineStore.Entities.User", "user")
                        .WithMany()
                        .HasForeignKey("FK_Like_User");

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("OnlineStore.Entities.Order", b =>
                {
                    b.HasOne("OnlineStore.Entities.User", "user")
                        .WithMany()
                        .HasForeignKey("FK_Order_User");

                    b.Navigation("user");
                });

            modelBuilder.Entity("OnlineStore.Entities.OrderDetail", b =>
                {
                    b.HasOne("OnlineStore.Entities.Order", "order")
                        .WithMany()
                        .HasForeignKey("FK_OrderDetail_order");

                    b.Navigation("order");
                });

            modelBuilder.Entity("OnlineStore.Entities.Product", b =>
                {
                    b.HasOne("OnlineStore.Entities.ProductBrand", "productBrand")
                        .WithMany()
                        .HasForeignKey("FK_Brand");

                    b.HasOne("OnlineStore.Entities.ProductAttribute", "productAttributes")
                        .WithMany()
                        .HasForeignKey("FK_Color");

                    b.Navigation("productAttributes");

                    b.Navigation("productBrand");
                });
#pragma warning restore 612, 618
        }
    }
}
