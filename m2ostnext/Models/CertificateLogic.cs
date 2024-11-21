// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.CertificateLogic
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{

  public class CertificateLogic
  {
    private MySqlConnection conn;

    public List<tbl_certificate_configure> get_Certificate_list()
    {
      List<tbl_certificate_configure> certificateConfigureList = new List<tbl_certificate_configure>();
      try
      {
        using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
          return m2ostDbContext.Database.SqlQuery<tbl_certificate_configure>("select * from tbl_certificate_configure where status = {0}", (object) "A").ToList<tbl_certificate_configure>();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public bool assignCertificate(tbl_certificate_assignment cert, int oid)
    {
      bool flag = false;
      try
      {
        using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
        {
          m2ostDbContext.Database.ExecuteSqlCommand("Insert into tbl_certificate_assignment(id_certificate,id_assessment,status,updated_date_time,id_org)values({0},{1},{2},{3},{4})", (object) cert.id_certificate, (object) cert.id_assessment, (object) "A", (object) DateTime.Now, (object) oid);
          flag = true;
          return flag;
        }
      }
      catch (Exception ex)
      {
        return flag;
      }
    }

    public List<tbl_certificate_assignment> getAssignList(int oid)
    {
      List<tbl_certificate_assignment> assignList = new List<tbl_certificate_assignment>();
      try
      {
        using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
          assignList = m2ostDbContext.Database.SqlQuery<tbl_certificate_assignment>("select * from tbl_certificate_assignment where status = {0} and id_org={1}", (object) "A", (object) oid).ToList<tbl_certificate_assignment>();
        if (assignList.Count > 0)
        {
          foreach (tbl_certificate_assignment certificateAssignment in assignList)
          {
            using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
            {
              certificateAssignment.assessment_title = m2ostDbContext.Database.SqlQuery<string>("select assessment_title from tbl_assessment where id_assessment = {0}", (object) certificateAssignment.id_assessment).FirstOrDefault<string>();
              certificateAssignment.certificate_title = m2ostDbContext.Database.SqlQuery<string>("select certificate_title from tbl_certificate_configure where id_certificate = {0}", (object) certificateAssignment.id_certificate).FirstOrDefault<string>();
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return assignList;
    }

    public bool DeleteAsssign(int id)
    {
      bool flag = false;
      try
      {
        using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
        {
          m2ostDbContext.Database.ExecuteSqlCommand("delete from  tbl_certificate_assignment where id_certificate_assignment={0}", (object) id);
          flag = true;
          return flag;
        }
      }
      catch (Exception ex)
      {
        return flag;
      }
    }

    }

  

public class CertificateAssignmentTheme
    {
        public int IdCertificate { get; set; }
        public int Id_organization { get; set; }

        public string SelectTheme { get; set; }
        public string HeaderThemeFirst { get; set; }
        public string SubText1ThemeFirst { get; set; }
        public string SubText2ThemeFirst { get; set; }
        public string SubText3ThemeFirst { get; set; }
        public string TextColorHeaderThemeFirst { get; set; }
        public string TextColorSubTextThemeFirst { get; set; }
        public string LogoImageLeftThemeFirst { get; set; }

        public HttpPostedFileBase LogoImageLeftThemeFirstUploadDir { get; set; }
        public string LogoImageRightThemeFirst { get; set; }

       
        public HttpPostedFileBase LogoImageRightThemeFirstUploadDir { get; set; }
        public string BackgroundImageThemeFirst { get; set; }

        public HttpPostedFileBase BackgroundImageThemeFirstUploadDir { get; set; }





        public string HeaderThemeSecond { get; set; }
        public string SubTextFirstThemeSecond { get; set; }
        public string RightNameThemeSecond { get; set; }
        public string LeftDesignationThemeSecond { get; set; }
        public string RightDepartmentThemeSecond { get; set; }
        public string LeftRegionThemeSecond { get; set; }
        public string SubText2ThemeSecond { get; set; }
        public string TextColorHeaderThemeSecond { get; set; }
        public string TextColorThemeSecond { get; set; }
        public string BackgroundImageThemeSecond { get; set; }
        public HttpPostedFileBase BackgroundImageThemeSecondUploadDir { get; set; }

        public string LogoImageLeftThemeSecond { get; set; }
        public HttpPostedFileBase LogoImageLeftThemeSecondUploadDir { get; set; }

        public string LogoImageRightThemeSecond { get; set; }
        public HttpPostedFileBase LogoImageRightThemeSecondUploadDir { get; set; }

    }

    public class CertificateLogicModel
    {
        private MySqlConnection conn;

        public CertificateLogicModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

        public string add_certificate(CertificateAssignmentTheme temp)
        {
            string str = (string)null;
            try
            {
                if (temp.IdCertificate != 0)
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "UPDATE tbl_certificate_assignment_theme SET Id_organization = @Id_organization, Select_theme = @Select_theme, Header_themefirst = @Header_themefirst, Sub_text1themefirst = @Sub_text1themefirst, Sub_text2themefirst = @Sub_text2themefirst, Sub_text3themefirst = @Sub_text3themefirst, Text_colorheaderthemfirst=@Text_colorheaderthemfirst ,Text_colorsubtextthemfirst=@Text_colorsubtextthemfirst,Logo_image_leftthemefirst=@Logo_image_leftthemefirst,Logo_image_rightthemefirst=@Logo_image_rightthemefirst,Backgrount_imagethemefirst=@Backgrount_imagethemefirst WHERE Id_certificate = " + temp.IdCertificate + "";
                    command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);
                    command.Parameters.AddWithValue("@Select_theme", temp.SelectTheme);
                    command.Parameters.AddWithValue("@Header_themefirst", temp.HeaderThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text1themefirst", temp.SubText1ThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text2themefirst", temp.SubText2ThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text3themefirst", temp.SubText3ThemeFirst);
                    command.Parameters.AddWithValue("@Text_colorheaderthemfirst", temp.TextColorHeaderThemeFirst);
                    command.Parameters.AddWithValue("@Text_colorsubtextthemfirst", temp.TextColorSubTextThemeFirst);
                    command.Parameters.AddWithValue("@Logo_image_leftthemefirst", temp.LogoImageLeftThemeFirst);
                    command.Parameters.AddWithValue("@Logo_image_rightthemefirst", temp.LogoImageRightThemeFirst);
                    command.Parameters.AddWithValue("@Backgrount_imagethemefirst", temp.BackgroundImageThemeFirst);
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }

                else
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "INSERT INTO tbl_certificate_assignment_theme (Id_organization,Select_theme, Header_themefirst, Sub_text1themefirst, Sub_text2themefirst, Sub_text3themefirst, Text_colorheaderthemfirst, Text_colorsubtextthemfirst, Logo_image_leftthemefirst, Logo_image_rightthemefirst, Backgrount_imagethemefirst) VALUES (@Id_organization,@Select_theme, @Header_themefirst, @Sub_text1themefirst, @Sub_text2themefirst, @Sub_text3themefirst, @Text_colorheaderthemfirst, @Text_colorsubtextthemfirst, @Logo_image_leftthemefirst, @Logo_image_rightthemefirst, @Backgrount_imagethemefirst)";
                    command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);
                    command.Parameters.AddWithValue("@Select_theme", temp.SelectTheme);
                    command.Parameters.AddWithValue("@Header_themefirst", temp.HeaderThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text1themefirst", temp.SubText1ThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text2themefirst", temp.SubText2ThemeFirst);
                    command.Parameters.AddWithValue("@Sub_text3themefirst", temp.SubText3ThemeFirst);
                    command.Parameters.AddWithValue("@Text_colorheaderthemfirst", temp.TextColorHeaderThemeFirst);
                    command.Parameters.AddWithValue("@Text_colorsubtextthemfirst", temp.TextColorSubTextThemeFirst);
                    command.Parameters.AddWithValue("@Logo_image_leftthemefirst", temp.LogoImageLeftThemeFirst);
                    command.Parameters.AddWithValue("@Logo_image_rightthemefirst", temp.LogoImageRightThemeFirst);
                    command.Parameters.AddWithValue("@Backgrount_imagethemefirst", temp.BackgroundImageThemeFirst);
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }

            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return str;
        }

        public List<CertificateAssignmentTheme> GetCertificateDataList(int num)
        {
            List<CertificateAssignmentTheme> certificateList = new List<CertificateAssignmentTheme>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"SELECT * FROM tbl_certificate_assignment_theme WHERE Status = 'A' AND Id_organization = '"+num+"'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CertificateAssignmentTheme certificate = new CertificateAssignmentTheme();
                            certificate.IdCertificate = Convert.ToInt32(reader["Id_Certificate"]);
                            certificate.SelectTheme = reader["Select_theme"].ToString();
                            certificate.HeaderThemeFirst = reader["Header_themefirst"].ToString();
                            certificate.SubText1ThemeFirst = reader["Sub_text1themefirst"].ToString();
                            certificate.SubText2ThemeFirst = reader["Sub_text2themefirst"].ToString();
                            certificate.SubText3ThemeFirst = reader["Sub_text3themefirst"].ToString();
                            certificate.TextColorHeaderThemeFirst = reader["Text_colorheaderthemfirst"].ToString();
                            certificate.TextColorSubTextThemeFirst = reader["Text_colorsubtextthemfirst"].ToString();
                            certificate.LogoImageLeftThemeFirst = reader["Logo_image_leftthemefirst"].ToString();
                            certificate.LogoImageRightThemeFirst = reader["Logo_image_rightthemefirst"].ToString();
                            certificate.BackgroundImageThemeFirst = reader["Backgrount_imagethemefirst"].ToString();

                            certificate.HeaderThemeSecond= reader["Header_themesecond"].ToString();
                            certificate.SubTextFirstThemeSecond = reader["Sub_textfirstthemsecond"].ToString();
                            certificate.RightNameThemeSecond = reader["Right_namethemesecond"].ToString();
                            certificate.LeftDesignationThemeSecond = reader["Left_designationthemesecond"].ToString();
                            certificate.RightDepartmentThemeSecond = reader["Right_depermentthemesecond"].ToString();
                            certificate.LeftRegionThemeSecond = reader["Left_regionthemesecond"].ToString();
                            certificate.SubText2ThemeSecond = reader["Sub_text2themsecond"].ToString();
                            certificate.TextColorHeaderThemeSecond = reader["Text_colorheaderthemesecond"].ToString();
                            certificate.TextColorThemeSecond = reader["Text_colorthemesecond"].ToString();
                            certificate.BackgroundImageThemeSecond = reader["Background_imagethemesecond"].ToString();
                            certificate.LogoImageLeftThemeSecond = reader["Logo_imageleftthemesecond"].ToString();
                            certificate.LogoImageRightThemeSecond = reader["Logo_imagerightthemesecond"].ToString();
                           
                            certificateList.Add(certificate);
                        }
                    }
                }
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }


            return certificateList;
        }

        public string add_certificate2(CertificateAssignmentTheme temp)
        {
            string str = (string)null;
            try
            {
                if (temp.IdCertificate != 0)
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "UPDATE tbl_certificate_assignment_theme SET Id_organization = @Id_organization,Select_theme = @Select_theme,Header_themesecond = @HeaderThemeSecond,Sub_textfirstthemsecond = @SubTextFirstThemeSecond,Right_namethemesecond = @RightNameThemeSecond,Left_designationthemesecond = @LeftDesignationThemeSecond,Right_depermentthemesecond = @RightDepartmentThemeSecond,Left_regionthemesecond = @LeftRegionThemeSecond,Sub_text2themsecond = @SubText2ThemeSecond,Text_colorheaderthemesecond = @TextColorHeaderThemeSecond,Text_colorthemesecond = @TextColorThemeSecond,Background_imagethemesecond = @BackgroundImageThemeSecond,Logo_imageleftthemesecond = @LogoImageLeftThemeSecond,Logo_imagerightthemesecond = @LogoImageRightThemeSecond WHERE Id_certificate = "+ temp.IdCertificate + "";
                   
                    command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);
                    command.Parameters.AddWithValue("@Select_theme", temp.SelectTheme);
                    command.Parameters.AddWithValue("@HeaderThemeSecond", temp.HeaderThemeSecond);
                    command.Parameters.AddWithValue("@SubTextFirstThemeSecond", temp.SubTextFirstThemeSecond);
                    command.Parameters.AddWithValue("@RightNameThemeSecond", temp.RightNameThemeSecond);
                    command.Parameters.AddWithValue("@LeftDesignationThemeSecond", temp.LeftDesignationThemeSecond);
                    command.Parameters.AddWithValue("@RightDepartmentThemeSecond", temp.RightDepartmentThemeSecond);
                    command.Parameters.AddWithValue("@LeftRegionThemeSecond", temp.LeftRegionThemeSecond);
                    command.Parameters.AddWithValue("@SubText2ThemeSecond", temp.SubText2ThemeSecond);
                    command.Parameters.AddWithValue("@TextColorHeaderThemeSecond", temp.TextColorHeaderThemeSecond);
                    command.Parameters.AddWithValue("@TextColorThemeSecond", temp.TextColorThemeSecond);
                    command.Parameters.AddWithValue("@BackgroundImageThemeSecond", temp.BackgroundImageThemeSecond);
                    command.Parameters.AddWithValue("@LogoImageLeftThemeSecond", temp.LogoImageLeftThemeSecond);
                    command.Parameters.AddWithValue("@LogoImageRightThemeSecond", temp.LogoImageRightThemeSecond);
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }

                else
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "INSERT INTO tbl_certificate_assignment_theme SET " +
                                          "Id_organization = @Id_organization, " +
                                          "Select_theme = @Select_theme, " +
                                          "Header_themesecond = @HeaderThemeSecond, " +
                                          "Sub_textfirstthemsecond = @SubTextFirstThemeSecond, " +
                                          "Right_namethemesecond = @RightNameThemeSecond, " +
                                          "Left_designationthemesecond = @LeftDesignationThemeSecond, " +
                                          "Right_depermentthemesecond = @RightDepartmentThemeSecond, " +
                                          "Left_regionthemesecond = @LeftRegionThemeSecond, " +
                                          "Sub_text2themsecond = @SubText2ThemeSecond, " +
                                          "Text_colorheaderthemesecond = @TextColorHeaderThemeSecond, " +
                                          "Text_colorthemesecond = @TextColorThemeSecond, " +

                                          "Background_imagethemesecond = @BackgroundImageThemeSecond, " +
                                          "Logo_imageleftthemesecond = @LogoImageLeftThemeSecond, " +
                                          "Logo_imagerightthemesecond = @LogoImageRightThemeSecond ";
                                         
                       command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);
                    command.Parameters.AddWithValue("@Select_theme", temp.SelectTheme);
                    command.Parameters.AddWithValue("@HeaderThemeSecond", temp.HeaderThemeSecond);
                    command.Parameters.AddWithValue("@SubTextFirstThemeSecond", temp.SubTextFirstThemeSecond);
                    command.Parameters.AddWithValue("@RightNameThemeSecond", temp.RightNameThemeSecond);
                    command.Parameters.AddWithValue("@LeftDesignationThemeSecond", temp.LeftDesignationThemeSecond);
                    command.Parameters.AddWithValue("@RightDepartmentThemeSecond", temp.RightDepartmentThemeSecond);
                    command.Parameters.AddWithValue("@LeftRegionThemeSecond", temp.LeftRegionThemeSecond);
                    command.Parameters.AddWithValue("@SubText2ThemeSecond", temp.SubText2ThemeSecond);
                    command.Parameters.AddWithValue("@TextColorHeaderThemeSecond", temp.TextColorHeaderThemeSecond);
                    command.Parameters.AddWithValue("@TextColorThemeSecond", temp.TextColorThemeSecond);
                    command.Parameters.AddWithValue("@BackgroundImageThemeSecond", temp.BackgroundImageThemeSecond);
                    command.Parameters.AddWithValue("@LogoImageLeftThemeSecond", temp.LogoImageLeftThemeSecond);
                    command.Parameters.AddWithValue("@LogoImageRightThemeSecond", temp.LogoImageRightThemeSecond);
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }

            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return str;
        }


    }

}
