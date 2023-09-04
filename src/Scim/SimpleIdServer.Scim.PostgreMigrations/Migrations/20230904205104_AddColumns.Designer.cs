﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SimpleIdServer.Scim.Persistence.EF;

#nullable disable

namespace SimpleIdServer.Scim.PostgreMigrations.Migrations
{
    [DbContext(typeof(SCIMDbContext))]
    [Migration("20230904205104_AddColumns")]
    partial class AddColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("scim")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SCIMRepresentationSCIMSchema", b =>
                {
                    b.Property<string>("RepresentationsId")
                        .HasColumnType("text");

                    b.Property<string>("SchemasId")
                        .HasColumnType("text");

                    b.HasKey("RepresentationsId", "SchemasId");

                    b.HasIndex("SchemasId");

                    b.ToTable("SCIMRepresentationSCIMSchema", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfiguration", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ResourceType")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ProvisioningConfigurations", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfigurationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Exception")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExecutionDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProvisioningConfigurationId")
                        .HasColumnType("text");

                    b.Property<string>("RepresentationId")
                        .HasColumnType("text");

                    b.Property<int>("RepresentationVersion")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("WorkflowId")
                        .HasColumnType("text");

                    b.Property<string>("WorkflowInstanceId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProvisioningConfigurationId");

                    b.ToTable("ProvisioningConfigurationHistory", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfigurationRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsArray")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ProvisioningConfigurationId")
                        .HasColumnType("text");

                    b.Property<int?>("ProvisioningConfigurationRecordId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("ValuesString")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProvisioningConfigurationId");

                    b.HasIndex("ProvisioningConfigurationRecordId");

                    b.ToTable("ProvisioningConfigurationRecord", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMAttributeMapping", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Mode")
                        .HasColumnType("integer");

                    b.Property<string>("SourceAttributeId")
                        .HasColumnType("text");

                    b.Property<string>("SourceAttributeSelector")
                        .HasColumnType("text");

                    b.Property<string>("SourceResourceType")
                        .HasColumnType("text");

                    b.Property<string>("TargetAttributeId")
                        .HasColumnType("text");

                    b.Property<string>("TargetResourceType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SCIMAttributeMappingLst", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMRepresentation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ResourceType")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SCIMRepresentationLst", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMRepresentationAttribute", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AttributeId")
                        .HasColumnType("text");

                    b.Property<string>("ComputedValueIndex")
                        .HasColumnType("text");

                    b.Property<string>("FullPath")
                        .HasColumnType("text");

                    b.Property<bool>("IsComputed")
                        .HasColumnType("boolean");

                    b.Property<string>("Namespace")
                        .HasColumnType("text");

                    b.Property<string>("ParentAttributeId")
                        .HasColumnType("text");

                    b.Property<string>("RepresentationId")
                        .HasColumnType("text");

                    b.Property<string>("ResourceType")
                        .HasColumnType("text");

                    b.Property<string>("SchemaAttributeId")
                        .HasColumnType("text");

                    b.Property<string>("ValueBinary")
                        .HasColumnType("text");

                    b.Property<bool?>("ValueBoolean")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ValueDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("ValueDecimal")
                        .HasColumnType("numeric");

                    b.Property<int?>("ValueInteger")
                        .HasColumnType("integer");

                    b.Property<string>("ValueReference")
                        .HasColumnType("text");

                    b.Property<string>("ValueString")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("ParentAttributeId");

                    b.HasIndex("RepresentationId");

                    b.HasIndex("SchemaAttributeId");

                    b.ToTable("SCIMRepresentationAttributeLst", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchema", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsRootSchema")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ResourceType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SCIMSchemaLst", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchemaAttribute", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CanonicalValues")
                        .HasColumnType("text");

                    b.Property<bool>("CaseExact")
                        .HasColumnType("boolean");

                    b.Property<string>("DefaultValueInt")
                        .HasColumnType("text");

                    b.Property<string>("DefaultValueString")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FullPath")
                        .HasColumnType("text");

                    b.Property<bool>("MultiValued")
                        .HasColumnType("boolean");

                    b.Property<int>("Mutability")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ParentId")
                        .HasColumnType("text");

                    b.Property<string>("ReferenceTypes")
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<int>("Returned")
                        .HasColumnType("integer");

                    b.Property<string>("SchemaId")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Uniqueness")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SchemaId");

                    b.ToTable("SCIMSchemaAttribute", "scim");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchemaExtension", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<string>("SCIMSchemaId")
                        .HasColumnType("text");

                    b.Property<string>("Schema")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SCIMSchemaId");

                    b.ToTable("SCIMSchemaExtension", "scim");
                });

            modelBuilder.Entity("SCIMRepresentationSCIMSchema", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMRepresentation", null)
                        .WithMany()
                        .HasForeignKey("RepresentationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMSchema", null)
                        .WithMany()
                        .HasForeignKey("SchemasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfigurationHistory", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.ProvisioningConfiguration", "ProvisioningConfiguration")
                        .WithMany("HistoryLst")
                        .HasForeignKey("ProvisioningConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ProvisioningConfiguration");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfigurationRecord", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.ProvisioningConfiguration", null)
                        .WithMany("Records")
                        .HasForeignKey("ProvisioningConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleIdServer.Scim.Domains.ProvisioningConfigurationRecord", null)
                        .WithMany("Values")
                        .HasForeignKey("ProvisioningConfigurationRecordId");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMRepresentationAttribute", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMRepresentationAttribute", null)
                        .WithMany("Children")
                        .HasForeignKey("ParentAttributeId");

                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMRepresentation", "Representation")
                        .WithMany("FlatAttributes")
                        .HasForeignKey("RepresentationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMSchemaAttribute", "SchemaAttribute")
                        .WithMany()
                        .HasForeignKey("SchemaAttributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Representation");

                    b.Navigation("SchemaAttribute");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchemaAttribute", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMSchema", null)
                        .WithMany("Attributes")
                        .HasForeignKey("SchemaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchemaExtension", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Domains.SCIMSchema", null)
                        .WithMany("SchemaExtensions")
                        .HasForeignKey("SCIMSchemaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfiguration", b =>
                {
                    b.Navigation("HistoryLst");

                    b.Navigation("Records");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.ProvisioningConfigurationRecord", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMRepresentation", b =>
                {
                    b.Navigation("FlatAttributes");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMRepresentationAttribute", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Domains.SCIMSchema", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("SchemaExtensions");
                });
#pragma warning restore 612, 618
        }
    }
}
