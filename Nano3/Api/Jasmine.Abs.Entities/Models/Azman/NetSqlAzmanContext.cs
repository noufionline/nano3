using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetSqlAzmanContext : DbContext
    {
        public virtual DbSet<AbsDivision> AbsDivisions { get; set; }
        public virtual DbSet<ApiClaim> ApiClaims { get; set; }
        public virtual DbSet<ApiResource> ApiResources { get; set; }
        public virtual DbSet<ApiScope> ApiScopes { get; set; }
        public virtual DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        public virtual DbSet<ApiSecret> ApiSecrets { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientClaim> ClientClaims { get; set; }
        public virtual DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public virtual DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public virtual DbSet<ClientIdPrestriction> ClientIdPrestrictions { get; set; }
        public virtual DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public virtual DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public virtual DbSet<ClientScope> ClientScopes { get; set; }
        public virtual DbSet<ClientSecret> ClientSecrets { get; set; }
        public virtual DbSet<IdServerLog> IdServerLogs { get; set; }
        public virtual DbSet<IdentityClaim> IdentityClaims { get; set; }
        public virtual DbSet<IdentityResource> IdentityResources { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<NetsqlazmanApplicationAttributesTable> NetsqlazmanApplicationAttributesTables { get; set; }
        public virtual DbSet<NetsqlazmanApplicationAttributesView> NetsqlazmanApplicationAttributesViews { get; set; }
        public virtual DbSet<NetsqlazmanApplicationGroupMembersTable> NetsqlazmanApplicationGroupMembersTables { get; set; }
        public virtual DbSet<NetsqlazmanApplicationGroupMembersView> NetsqlazmanApplicationGroupMembersViews { get; set; }
        public virtual DbSet<NetsqlazmanApplicationGroupsTable> NetsqlazmanApplicationGroupsTables { get; set; }
        public virtual DbSet<NetsqlazmanApplicationPermissionsTable> NetsqlazmanApplicationPermissionsTables { get; set; }
        public virtual DbSet<NetsqlazmanApplicationsTable> NetsqlazmanApplicationsTables { get; set; }
        public virtual DbSet<NetsqlazmanApplicationsView> NetsqlazmanApplicationsViews { get; set; }
        public virtual DbSet<NetsqlazmanAuthorizationAttributesTable> NetsqlazmanAuthorizationAttributesTables { get; set; }
        public virtual DbSet<NetsqlazmanAuthorizationAttributesView> NetsqlazmanAuthorizationAttributesViews { get; set; }
        public virtual DbSet<NetsqlazmanAuthorizationView> NetsqlazmanAuthorizationViews { get; set; }
        public virtual DbSet<NetsqlazmanAuthorizationsTable> NetsqlazmanAuthorizationsTables { get; set; }
        public virtual DbSet<NetsqlazmanBizRuleView> NetsqlazmanBizRuleViews { get; set; }
        public virtual DbSet<NetsqlazmanBizRulesTable> NetsqlazmanBizRulesTables { get; set; }
        public virtual DbSet<NetsqlazmanDatabaseUser> NetsqlazmanDatabaseUsers { get; set; }
        public virtual DbSet<NetsqlazmanItemAttributesTable> NetsqlazmanItemAttributesTables { get; set; }
        public virtual DbSet<NetsqlazmanItemAttributesView> NetsqlazmanItemAttributesViews { get; set; }
        public virtual DbSet<NetsqlazmanItemsHierarchyTable> NetsqlazmanItemsHierarchyTables { get; set; }
        public virtual DbSet<NetsqlazmanItemsHierarchyView> NetsqlazmanItemsHierarchyViews { get; set; }
        public virtual DbSet<NetsqlazmanItemsTable> NetsqlazmanItemsTables { get; set; }
        public virtual DbSet<NetsqlazmanLogTable> NetsqlazmanLogTables { get; set; }
        public virtual DbSet<NetsqlazmanSetting> NetsqlazmanSettings { get; set; }
        public virtual DbSet<NetsqlazmanStoreAttributesTable> NetsqlazmanStoreAttributesTables { get; set; }
        public virtual DbSet<NetsqlazmanStoreAttributesView> NetsqlazmanStoreAttributesViews { get; set; }
        public virtual DbSet<NetsqlazmanStoreGroupMembersTable> NetsqlazmanStoreGroupMembersTables { get; set; }
        public virtual DbSet<NetsqlazmanStoreGroupMembersView> NetsqlazmanStoreGroupMembersViews { get; set; }
        public virtual DbSet<NetsqlazmanStoreGroupsTable> NetsqlazmanStoreGroupsTables { get; set; }
        public virtual DbSet<NetsqlazmanStorePermissionsTable> NetsqlazmanStorePermissionsTables { get; set; }
        public virtual DbSet<NetsqlazmanStoresTable> NetsqlazmanStoresTables { get; set; }
        public virtual DbSet<OdataLog> OdataLogs { get; set; }
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }
        public virtual DbSet<ReportApiLog> ReportApiLogs { get; set; }
        public virtual DbSet<UsersDemo> UsersDemoes { get; set; }

        public NetSqlAzmanContext(DbContextOptions<NetSqlAzmanContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsDivision>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ApplicationType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.InitialCatelog)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ApiClaim>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiClaims)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiResource>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ApiScope>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiScopes)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiScopeClaim>(entity =>
            {
                entity.HasIndex(e => e.ApiScopeId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiScope)
                    .WithMany(p => p.ApiScopeClaims)
                    .HasForeignKey(d => d.ApiScopeId);
            });

            modelBuilder.Entity<ApiSecret>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Type).HasMaxLength(250);

                entity.Property(e => e.Value).HasMaxLength(2000);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiSecrets)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspNetUsers_AbsDivisions_Id");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.ClientUri).HasMaxLength(2000);

                entity.Property(e => e.ProtocolType)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ClientClaim>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientClaims)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientCorsOrigin>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCorsOrigins)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientGrantType>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.GrantType)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientGrantTypes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientIdPrestriction>(entity =>
            {
                entity.ToTable("ClientIdPRestrictions");

                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientIdPrestrictions)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.PostLogoutRedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPostLogoutRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientRedirectUri>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.RedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientScope>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientScopes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientSecret>(entity =>
            {
                entity.HasIndex(e => e.ClientId);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Type).HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientSecrets)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<IdServerLog>(entity =>
            {
                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.MessageTemplate).IsRequired();

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<IdentityClaim>(entity =>
            {
                entity.HasIndex(e => e.IdentityResourceId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.IdentityResource)
                    .WithMany(p => p.IdentityClaims)
                    .HasForeignKey(d => d.IdentityResourceId);
            });

            modelBuilder.Entity<IdentityResource>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<NetsqlazmanApplicationAttributesTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationAttributeId)
                    .HasName("PK_ApplicationAttributes");

                entity.ToTable("netsqlazman_ApplicationAttributesTable");

                entity.HasIndex(e => new { e.ApplicationId, e.AttributeKey })
                    .HasName("IX_ApplicationAttributes");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.NetsqlazmanApplicationAttributesTables)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationAttributes_Applications");
            });

            modelBuilder.Entity<NetsqlazmanApplicationAttributesView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_ApplicationAttributesView");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanApplicationGroupMembersTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationGroupMemberId)
                    .HasName("PK_GroupMembers");

                entity.ToTable("netsqlazman_ApplicationGroupMembersTable");

                entity.HasIndex(e => new { e.ApplicationGroupId, e.ObjectSid })
                    .HasName("IX_ApplicationGroupMembers");

                entity.HasIndex(e => new { e.ApplicationGroupId, e.ObjectSid, e.IsMember })
                    .HasName("ApplicationGroupMembers_ApplicationGroupId_ObjectSid_IsMember_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.HasOne(d => d.ApplicationGroup)
                    .WithMany(p => p.NetsqlazmanApplicationGroupMembersTables)
                    .HasForeignKey(d => d.ApplicationGroupId)
                    .HasConstraintName("FK_ApplicationGroupMembers_ApplicationGroup");
            });

            modelBuilder.Entity<NetsqlazmanApplicationGroupMembersView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_ApplicationGroupMembersView");

                entity.Property(e => e.ApplicationGroup)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.Property(e => e.WhereDefined)
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NetsqlazmanApplicationGroupsTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationGroupId)
                    .HasName("PK_Groups");

                entity.ToTable("netsqlazman_ApplicationGroupsTable");

                entity.HasIndex(e => e.ObjectSid)
                    .HasName("IX_ApplicationGroups_1");

                entity.HasIndex(e => new { e.ApplicationId, e.Name })
                    .HasName("IX_ApplicationGroups");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.LdapQuery)
                    .HasColumnName("LDapQuery")
                    .HasMaxLength(4000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.NetsqlazmanApplicationGroupsTables)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationGroups_Applications");
            });

            modelBuilder.Entity<NetsqlazmanApplicationPermissionsTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationPermissionId)
                    .HasName("PK_ApplicationPermissions");

                entity.ToTable("netsqlazman_ApplicationPermissionsTable");

                entity.HasIndex(e => e.ApplicationId)
                    .HasName("IX_ApplicationPermissions");

                entity.HasIndex(e => new { e.ApplicationId, e.SqlUserOrRole, e.NetSqlAzManFixedServerRole })
                    .HasName("IX_ApplicationPermissions_1");

                entity.Property(e => e.SqlUserOrRole)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.NetsqlazmanApplicationPermissionsTables)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationPermissions_ApplicationsTable");
            });

            modelBuilder.Entity<NetsqlazmanApplicationsTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .HasName("PK_Applications");

                entity.ToTable("netsqlazman_ApplicationsTable");

                entity.HasIndex(e => new { e.ApplicationId, e.Name })
                    .HasName("IX_Applications");

                entity.HasIndex(e => new { e.Name, e.StoreId })
                    .HasName("Applications_StoreId_Name_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.NetsqlazmanApplicationsTables)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Applications_Stores");
            });

            modelBuilder.Entity<NetsqlazmanApplicationsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_ApplicationsView");

                entity.Property(e => e.ApplicationDescription)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StoreDescription)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanAuthorizationAttributesTable>(entity =>
            {
                entity.HasKey(e => e.AuthorizationAttributeId)
                    .HasName("PK_AuthorizationAttributes");

                entity.ToTable("netsqlazman_AuthorizationAttributesTable");

                entity.HasIndex(e => new { e.AuthorizationId, e.AttributeKey })
                    .HasName("IX_AuthorizationAttributes");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Authorization)
                    .WithMany(p => p.NetsqlazmanAuthorizationAttributesTables)
                    .HasForeignKey(d => d.AuthorizationId)
                    .HasConstraintName("FK_AuthorizationAttributes_Authorizations");
            });

            modelBuilder.Entity<NetsqlazmanAuthorizationAttributesView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_AuthorizationAttributesView");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.AuthorizationType)
                    .HasMaxLength(19)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.Property(e => e.Owner).HasMaxLength(255);

                entity.Property(e => e.SidWhereDefined)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<NetsqlazmanAuthorizationView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_AuthorizationView");

                entity.Property(e => e.AuthorizationType)
                    .HasMaxLength(19)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.Property(e => e.Owner).HasMaxLength(255);

                entity.Property(e => e.SidWhereDefined)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<NetsqlazmanAuthorizationsTable>(entity =>
            {
                entity.HasKey(e => e.AuthorizationId)
                    .HasName("PK_Authorizations");

                entity.ToTable("netsqlazman_AuthorizationsTable");

                entity.HasIndex(e => new { e.ItemId, e.ObjectSid })
                    .HasName("IX_Authorizations");

                entity.HasIndex(e => new { e.ItemId, e.ObjectSid, e.ObjectSidWhereDefined, e.AuthorizationType, e.ValidFrom, e.ValidTo })
                    .HasName("IX_Authorizations_1");

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.Property(e => e.ObjectSidWhereDefined).HasColumnName("objectSidWhereDefined");

                entity.Property(e => e.OwnerSid)
                    .IsRequired()
                    .HasColumnName("ownerSid")
                    .HasMaxLength(85);

                entity.Property(e => e.OwnerSidWhereDefined).HasColumnName("ownerSidWhereDefined");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.NetsqlazmanAuthorizationsTables)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Authorizations_Items");
            });

            modelBuilder.Entity<NetsqlazmanBizRuleView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_BizRuleView");

                entity.Property(e => e.BizRuleSource)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CompiledAssembly)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanBizRulesTable>(entity =>
            {
                entity.HasKey(e => e.BizRuleId)
                    .HasName("PK_BizRules");

                entity.ToTable("netsqlazman_BizRulesTable");

                entity.Property(e => e.BizRuleSource)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CompiledAssembly)
                    .IsRequired()
                    .HasColumnType("image");
            });

            modelBuilder.Entity<NetsqlazmanDatabaseUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_DatabaseUsers");

                entity.Property(e => e.DbuserName)
                    .HasColumnName("DBUserName")
                    .HasMaxLength(256);

                entity.Property(e => e.DbuserSid)
                    .HasColumnName("DBUserSid")
                    .HasMaxLength(85);

                entity.Property(e => e.OtherFields)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NetsqlazmanItemAttributesTable>(entity =>
            {
                entity.HasKey(e => e.ItemAttributeId)
                    .HasName("PK_ItemAttributes");

                entity.ToTable("netsqlazman_ItemAttributesTable");

                entity.HasIndex(e => new { e.ItemId, e.AttributeKey })
                    .HasName("IX_ItemAttributes");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.NetsqlazmanItemAttributesTables)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ItemAttributes_Items");
            });

            modelBuilder.Entity<NetsqlazmanItemAttributesView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_ItemAttributesView");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.ItemType)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanItemsHierarchyTable>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.MemberOfItemId })
                    .HasName("PK_ItemsHierarchy");

                entity.ToTable("netsqlazman_ItemsHierarchyTable");

                entity.HasIndex(e => e.ItemId)
                    .HasName("IX_ItemsHierarchy");

                entity.HasIndex(e => e.MemberOfItemId)
                    .HasName("IX_ItemsHierarchy_1");
            });

            modelBuilder.Entity<NetsqlazmanItemsHierarchyView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_ItemsHierarchyView");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.ItemType)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.MemberDescription)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberType)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanItemsTable>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PK_Items");

                entity.ToTable("netsqlazman_ItemsTable");

                entity.HasIndex(e => new { e.ApplicationId, e.Name })
                    .HasName("IX_Items");

                entity.HasIndex(e => new { e.Name, e.ApplicationId })
                    .HasName("Items_ApplicationId_Name_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.NetsqlazmanItemsTables)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_Items_Applications");

                entity.HasOne(d => d.BizRule)
                    .WithMany(p => p.NetsqlazmanItemsTables)
                    .HasForeignKey(d => d.BizRuleId)
                    .HasConstraintName("FK_Items_BizRules");
            });

            modelBuilder.Entity<NetsqlazmanLogTable>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK_Log")
                    .IsClustered(false);

                entity.ToTable("netsqlazman_LogTable");

                entity.HasIndex(e => e.SqlIdentity)
                    .HasName("IX_Log_1");

                entity.HasIndex(e => e.WindowsIdentity)
                    .HasName("IX_Log");

                entity.HasIndex(e => new { e.LogDateTime, e.InstanceGuid, e.OperationCounter })
                    .HasName("IX_Log_2")
                    .IsClustered();

                entity.Property(e => e.Ensdescription)
                    .IsRequired()
                    .HasColumnName("ENSDescription")
                    .HasMaxLength(4000);

                entity.Property(e => e.Enstype)
                    .IsRequired()
                    .HasColumnName("ENSType")
                    .HasMaxLength(255);

                entity.Property(e => e.LogDateTime).HasColumnType("datetime");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SqlIdentity)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.WindowsIdentity)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanSetting>(entity =>
            {
                entity.HasKey(e => e.SettingName)
                    .HasName("PK_Settings");

                entity.ToTable("netsqlazman_Settings");

                entity.Property(e => e.SettingName).HasMaxLength(255);

                entity.Property(e => e.SettingValue)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanStoreAttributesTable>(entity =>
            {
                entity.HasKey(e => e.StoreAttributeId)
                    .HasName("PK_StoreAttributes");

                entity.ToTable("netsqlazman_StoreAttributesTable");

                entity.HasIndex(e => new { e.StoreId, e.AttributeKey })
                    .HasName("StoreAttributes_AuhorizationId_AttributeKey_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.NetsqlazmanStoreAttributesTables)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StoreAttributes_Stores");
            });

            modelBuilder.Entity<NetsqlazmanStoreAttributesView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_StoreAttributesView");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NetsqlazmanStoreGroupMembersTable>(entity =>
            {
                entity.HasKey(e => e.StoreGroupMemberId)
                    .HasName("PK_StoreGroupMembers");

                entity.ToTable("netsqlazman_StoreGroupMembersTable");

                entity.HasIndex(e => new { e.StoreGroupId, e.ObjectSid })
                    .HasName("IX_StoreGroupMembers");

                entity.HasIndex(e => new { e.StoreGroupId, e.ObjectSid, e.IsMember })
                    .HasName("StoreGroupMembers_StoreGroupId_ObjectSid_IsMember_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.HasOne(d => d.StoreGroup)
                    .WithMany(p => p.NetsqlazmanStoreGroupMembersTables)
                    .HasForeignKey(d => d.StoreGroupId)
                    .HasConstraintName("FK_StoreGroupMembers_StoreGroup");
            });

            modelBuilder.Entity<NetsqlazmanStoreGroupMembersView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("netsqlazman_StoreGroupMembersView");

                entity.Property(e => e.MemberType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.Property(e => e.StoreGroup)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.WhereDefined)
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NetsqlazmanStoreGroupsTable>(entity =>
            {
                entity.HasKey(e => e.StoreGroupId)
                    .HasName("PK_StoreGroups");

                entity.ToTable("netsqlazman_StoreGroupsTable");

                entity.HasIndex(e => new { e.StoreId, e.Name })
                    .HasName("StoreGroups_StoreId_Name_Unique_Index")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.ObjectSid })
                    .HasName("IX_StoreGroups");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.LdapQuery)
                    .HasColumnName("LDapQuery")
                    .HasMaxLength(4000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasColumnName("objectSid")
                    .HasMaxLength(85);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.NetsqlazmanStoreGroupsTables)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StoreGroups_Stores");
            });

            modelBuilder.Entity<NetsqlazmanStorePermissionsTable>(entity =>
            {
                entity.HasKey(e => e.StorePermissionId)
                    .HasName("PK_StorePermissions");

                entity.ToTable("netsqlazman_StorePermissionsTable");

                entity.HasIndex(e => e.StoreId)
                    .HasName("IX_StorePermissions");

                entity.HasIndex(e => new { e.StoreId, e.SqlUserOrRole, e.NetSqlAzManFixedServerRole })
                    .HasName("IX_StorePermissions_1");

                entity.Property(e => e.SqlUserOrRole)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.NetsqlazmanStorePermissionsTables)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StorePermissions_StoresTable");
            });

            modelBuilder.Entity<NetsqlazmanStoresTable>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("PK_Stores");

                entity.ToTable("netsqlazman_StoresTable");

                entity.HasIndex(e => e.Name)
                    .HasName("Stores_Name_Unique_Index")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<OdataLog>(entity =>
            {
                entity.ToTable("ODataLogs");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.MessageTemplate).IsRequired();

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Type });

                entity.HasIndex(e => e.SubjectId);

                entity.HasIndex(e => new { e.SubjectId, e.ClientId });

                entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type });

                entity.Property(e => e.Key).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.SubjectId).HasMaxLength(200);
            });

            modelBuilder.Entity<ReportApiLog>(entity =>
            {
                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.MessageTemplate).IsRequired();

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<UsersDemo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UsersDemo");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OtherFields).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
