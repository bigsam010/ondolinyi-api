using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OndoLinyiApi.Models
{
    public partial class ondolinyiContext : DbContext
    {
        public ondolinyiContext()
        {
        }

        public ondolinyiContext(DbContextOptions<ondolinyiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerAddress> Customeraddress { get; set; }
        public virtual DbSet<Customersettings> Customersettings { get; set; }
        public virtual DbSet<Customercard> Customercard { get; set; }
        public virtual DbSet<Dispatcher> Dispatcher { get; set; }
        public virtual DbSet<Loginentry> Loginentry { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Orderrequest> Orderrequest { get; set; }
        public virtual DbSet<Passwordreset> Passwordreset { get; set; }
        public virtual DbSet<Paymentlog> Paymentlog { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Productcategory> Productcategory { get; set; }
        public virtual DbSet<Productimages> Productimages { get; set; }
        public virtual DbSet<Pricerangemodel> Pricerangemodel { get; set; }
        public virtual DbSet<Multiplepricerangemodel> Multiplepricerangemodel { get; set; }
        public virtual DbSet<Reqinvoice> Reqinvoice { get; set; }
        public virtual DbSet<Rfq> Rfq { get; set; }
        public virtual DbSet<Rfqchatlog> Rfqchatlog { get; set; }
        public virtual DbSet<Rfqchatresponse> Rfqchatresponse { get; set; }
        public virtual DbSet<Rfqreciept> Rfqreciept { get; set; }
        public virtual DbSet<Stockhistory> Stockhistory { get; set; }
        public virtual DbSet<Salesrepsurvey> Salesrepsurvey { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseSqlServer("Server=DESKTOP-B8QGDUA\\SQLEXPRESS;Database=ondolinyi;Trusted_Connection=True;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__customer__AB6E616531E13318");

                entity.ToTable("customer");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Isverified).HasColumnName("isverified");

                entity.Property(e => e.Joindate)
                    .HasColumnName("joindate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("text");




                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Verificationtoken)
                    .HasColumnName("verificationtoken")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Customercard>(entity =>
            {
                entity.HasKey(e => e.Cardnumber)
                    .HasName("PK__customer__C2957E4318BD4AA2");

                entity.ToTable("customercard");

                entity.Property(e => e.Cardnumber)
                    .HasColumnName("cardnumber")
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cardholder)
                    .IsRequired()
                    .HasColumnName("cardholder")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasColumnName("cvv")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Expdate)
                    .HasColumnName("expdate")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__customer__3213E83F0744B298");

                entity.ToTable("customeraddress");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                   .HasColumnName("state")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.Country)
                   .HasColumnName("country")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.Phone1)
                   .IsRequired()
                   .HasColumnName("phone1")
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.Phone2)
                    .HasColumnName("phone2")
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.Isdefault)
                    .HasColumnName("isdefault");
            });


            modelBuilder.Entity<Customersettings>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__customer__3213E83F94B23303");

                entity.ToTable("customersettings");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id");


                entity.Property(e => e.Allowsms)
                   .HasColumnName("allowsms");

                entity.Property(e => e.Allowemail)
                    .HasColumnName("allowemail");

                entity.Property(e => e.Notifyorderstatus)
                   .HasColumnName("notifyorderstatus");



            });



            modelBuilder.Entity<Dispatcher>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__dispatch__3213E83FD884B242");

                entity.ToTable("dispatcher");

                entity.Property(e => e.Id)
                    .HasColumnName("id");


                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Orderid)
                     .IsRequired()
                     .HasColumnName("orderid")
                     .HasMaxLength(12)
                     .IsUnicode(false);
            });



            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("manufacturer");

                entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");

                entity.Property(e => e.Aboutcompany)
                    .HasColumnName("aboutcompany")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Companyaddress)
                    .HasColumnName("companyaddress")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Companyemail)
                    .IsRequired()
                    .HasColumnName("companyemail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Companyname)
                    .IsRequired()
                    .HasColumnName("companyname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Companyphone)
                    .HasColumnName("companyphone")
                    .HasMaxLength(11)
                    .IsUnicode(false);


                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rcno)
                    .HasColumnName("rcno")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Legalname)
                    .HasColumnName("legalname")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.Notifyid)
                    .HasName("PK__notifica__BA7DF1AE34131B6D");

                entity.ToTable("notifications");

                entity.Property(e => e.Notifyid).HasColumnName("notifyid");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Notifydate)
                    .HasColumnName("notifydate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Viewed).HasColumnName("viewed");
            });

            modelBuilder.Entity<Orderrequest>(entity =>
            {
                entity.HasKey(e => e.Orderid)
                    .HasName("PK__orderreq__080E37758244FCCF");

                entity.ToTable("orderrequest");

                entity.Property(e => e.Orderid)
                    .HasColumnName("orderid")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Deliverydate)
                    .HasColumnName("deliverydate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Requestdate)
                    .HasColumnName("requestdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Reviewed).HasColumnName("reviewed");

                entity.Property(e => e.Rfqid)
                    .IsRequired()
                    .HasColumnName("rfqid")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Productid)
                .IsRequired()
                .HasColumnName("productid");
            });

            modelBuilder.Entity<Passwordreset>(entity =>
            {
                entity.HasKey(e => e.Reqid)
                    .HasName("PK__password__20C372019FF6EDD6");

                entity.ToTable("passwordreset");

                entity.Property(e => e.Reqid).HasColumnName("reqid");

                entity.Property(e => e.Customer)
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expdate)
                    .HasColumnName("expdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Reqdate)
                    .HasColumnName("reqdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Paymentlog>(entity =>
            {
                entity.HasKey(e => e.Refno)
                    .HasName("PK__paymentl__19842C59BB9C8CAC");

                entity.ToTable("paymentlog");

                entity.Property(e => e.Refno)
                    .HasColumnName("refno")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Card)
                    .IsRequired()
                    .HasColumnName("card")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Invoicenumber)
                    .IsRequired()
                    .HasColumnName("invoicenumber")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Paymentdate)
                    .HasColumnName("paymentdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Mode)
                 .HasColumnName("mode")
                 .HasMaxLength(6)
                 .IsUnicode(false);

                entity.Property(e => e.Totalamount)
                 .HasColumnName("totalamount")
                 .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Rfqid)
                .HasColumnName("rfqid")
                .HasMaxLength(12)
                .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dateadded)
                    .HasColumnName("dateadded")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");




                entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");

                entity.Property(e => e.Measureunit)
                    .HasColumnName("measureunit")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Moq).HasColumnName("moq");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Qtyavailable).HasColumnName("qtyavailable");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Surveywaitingdays).HasColumnName("surveywaitingdays");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Taxtype)
                  .HasColumnName("taxtype");

                entity.Property(e => e.Pricemodel)
                .HasColumnName("pricemodel")
                .HasMaxLength(14)
                .IsUnicode(false);

                entity.Property(e => e.Pagetitle)
               .HasColumnName("pagetitle")
               .HasMaxLength(200)
               .IsUnicode(false);

                entity.Property(e => e.Mappedurl)
               .HasColumnName("mappedurl")
               .HasMaxLength(100)
               .IsUnicode(false);

                entity.Property(e => e.Seodescription)
               .HasColumnName("seodescription")
               .HasMaxLength(500)
               .IsUnicode(false);

                entity.Property(e => e.Sku)
               .HasColumnName("sku");


            });

            modelBuilder.Entity<Productcategory>(entity =>
            {
                entity.HasKey(e => e.Catid)
                    .HasName("PK__productc__17B9D93EC2FD8A70");

                entity.ToTable("productcategory");

                entity.Property(e => e.Catid).HasColumnName("catid");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pricerangemodel>(entity =>
            {
                entity.HasKey(e => e.Productid)
                    .HasName("PK__priceran__2D172D326E14CCE2");

                entity.ToTable("pricerangemodel");

                entity.Property(e => e.Lowprice)
                .HasColumnName("lowprice")
                .HasColumnType("decimal(10,2)");

            });

            modelBuilder.Entity<Multiplepricerangemodel>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__multiple__3213E83FF3CEBC21");

                entity.ToTable("multiplepricerangemodel");

                entity.Property(e => e.Productid)
                .HasColumnName("productid");

                entity.Property(e => e.Lowerbound)
               .HasColumnName("lowerbound");

                entity.Property(e => e.Upperbound)
               .HasColumnName("upperbound");

                entity.Property(e => e.Price)
               .HasColumnName("price")
                .HasColumnType("decimal(10,2)");


            });

            modelBuilder.Entity<Productimages>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__producti__3213E83FABAA477A");

                entity.ToTable("productimages");

                entity.Property(e => e.Productid)
                .HasColumnName("productid");

                entity.Property(e => e.Imgname)
               .HasColumnName("imgname")
               .HasMaxLength(30)
               .IsUnicode(false);

                entity.Property(e => e.Type)
               .HasColumnName("type")
               .HasMaxLength(6)
               .IsUnicode(false);

            });

            modelBuilder.Entity<Reqinvoice>(entity =>
            {
                entity.HasKey(e => e.Invoicenubmer)
                    .HasName("PK__reqinvoi__C4D8A6A0F354DB4F");

                entity.ToTable("reqinvoice");

                entity.Property(e => e.Invoicenubmer)
                    .HasColumnName("invoicenubmer")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Deliverycost)
                    .HasColumnName("deliverycost")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Productprice)
                    .HasColumnName("productprice")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Rfqid)
                    .IsRequired()
                    .HasColumnName("rfqid")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Taxamount)
                    .HasColumnName("taxamount")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Totalamount)
                .HasColumnName("totalamount")
                .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Quantity)
                 .HasColumnName("quantity");
            });

            modelBuilder.Entity<Rfq>(entity =>
            {
                entity.ToTable("rfq");

                entity.Property(e => e.Rfqid)
                    .HasColumnName("rfqid")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasColumnName("customer")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Requestdate)
                    .HasColumnName("requestdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Specification)
                    .HasColumnName("specification")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                   .HasColumnName("address")
                   .HasMaxLength(300)
                   .IsUnicode(false);

                entity.Property(e => e.State)
                   .HasColumnName("state")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.Salesrep)
                .HasColumnName("salesrep")
                .HasMaxLength(50)
                .IsUnicode(false);
            });

            modelBuilder.Entity<Rfqchatlog>(entity =>
            {
                entity.HasKey(e => e.Chatid)
                    .HasName("PK__rfqchatl__826281958DAEE5C3");

                entity.ToTable("rfqchatlog");

                entity.Property(e => e.Chatid).HasColumnName("chatid");

                entity.Property(e => e.Chaatdate)
                    .HasColumnName("chaatdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Salesrep)
                    .IsRequired()
                    .HasColumnName("Salesrep")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rfqid)
                    .IsRequired()
                    .HasColumnName("rfqid")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Viewed).HasColumnName("viewed");
            });

            modelBuilder.Entity<Rfqchatresponse>(entity =>
            {
                entity.HasKey(e => e.Chatresid)
                    .HasName("PK__rfqchatr__5DAC38228C674646");

                entity.ToTable("rfqchatresponse");

                entity.Property(e => e.Chatresid).HasColumnName("chatresid");

                entity.Property(e => e.Chatid).HasColumnName("chatid");

                entity.Property(e => e.Reponse)
                    .IsRequired()
                    .HasColumnName("reponse")
                    .HasColumnType("text");

                entity.Property(e => e.Resby)
                    .IsRequired()
                    .HasColumnName("resby")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Resdate)
                    .HasColumnName("resdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Viewed).HasColumnName("viewed");
            });

            modelBuilder.Entity<Rfqreciept>(entity =>
            {
                entity.HasKey(e => e.Receiptnumber)
                    .HasName("PK__rfqrecie__18A58C1274434B77");

                entity.ToTable("rfqreciept");

                entity.Property(e => e.Receiptnumber)
                    .HasColumnName("receiptnumber")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Gendate)
                    .HasColumnName("gendate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Refno)
                    .IsRequired()
                    .HasColumnName("refno")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stockhistory>(entity =>
            {
                entity.HasKey(e => e.Logid)
                    .HasName("PK__stockhis__7838F26523DD8867");

                entity.ToTable("stockhistory");

                entity.Property(e => e.Logid).HasColumnName("logid");

                entity.Property(e => e.Addedby)
                    .IsRequired()
                    .HasColumnName("addedby")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dateadded)
                    .HasColumnName("dateadded")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Qtyadded).HasColumnName("qtyadded");

                entity.Property(e => e.Qtyavailable).HasColumnName("qtyavailable");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("survey");

                entity.Property(e => e.Surveyid).HasColumnName("surveyid");

                entity.Property(e => e.Orderid).HasColumnName("orderid").HasMaxLength(12).IsUnicode(false);

                entity.Property(e => e.Rating)
                    .HasColumnName("rating");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Surveyby)
                    .IsRequired()
                    .HasColumnName("surveyby")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surveydate)
                    .HasColumnName("surveydate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                  .IsRequired()
                  .HasColumnName("status")
                  .HasMaxLength(7)
                  .IsUnicode(false);
            });


            modelBuilder.Entity<Salesrepsurvey>(entity =>
            {
                entity.ToTable("salesrepsurvey");
                entity.HasKey(e => e.Surveyid).HasName("PK__salesrep__B313676212F9D25E");

                entity.Property(e => e.Surveyid).HasColumnName("surveyid");

                entity.Property(e => e.SalesRep).HasColumnName("salesRep").HasMaxLength(12).IsUnicode(false);

                entity.Property(e => e.Rating)
                    .HasColumnName("rating");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Surveyby)
                    .IsRequired()
                    .HasColumnName("surveyby")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surveydate)
                    .HasColumnName("surveydate")
                    .HasColumnType("datetime");


            });

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.ToTable("tax");

                entity.Property(e => e.Taxid).HasColumnName("taxid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Invoicelabel)
                  .IsRequired()
                  .HasColumnName("invoicelabel")
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.Description)
                  .IsRequired()
                  .HasColumnName("description")
                  .HasMaxLength(200)
                  .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__users__AB6E6165BBF756CA");

                entity.ToTable("users");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Joindate)
                    .HasColumnName("joindate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                .IsRequired()
                .HasColumnName("phone")
                .HasMaxLength(11)
                .IsUnicode(false);

                entity.Property(e => e.Bio)
                .IsRequired()
                .HasColumnName("bio")
                .HasMaxLength(200)
                .IsUnicode(false);

                entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password")
                .HasMaxLength(500)
                .IsUnicode(false);

            });

            modelBuilder.Entity<Loginentry>(entity =>
            {
                entity.ToTable("loginentry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client)
                    .IsRequired()
                    .HasColumnName("client")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Logindate)
                    .HasColumnName("logindate")
                    .HasColumnType("datetime");
            });
        }
    }
}
