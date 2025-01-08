using SISGESAL.web.Data.Entities;
using SISGESAL.web.Enums;
using SISGESAL.web.Helpers;

namespace SISGESAL.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context , IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _dataContext = context;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRoleAsync();
            var manager = await CheckUserAsync("ADMIN", "admin@admin.com", "admin", UserType.Manager);
            var customer = await CheckUserAsync("USER", "user@user.com", "user", UserType.Customer);

            await CheckDepartmentsAsync();

            await CheckCustomerAsync(customer);
            await CheckManagerAsync(manager);
        }

        private async Task<User> CheckUserAsync(string fullName, string email, string username, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(username);
            if (user == null)
            {
                user = new User
                {
                    FullName = fullName,
                    Email = email,
                    UserName = username,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "Indep3NdeNCI@");
                await _userHelper.AddUserToRoleAsync(user, userType);
            }
            return user;
        }

        private async Task CheckRoleAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Manager.ToString());
            await _userHelper.CheckRoleAsync(UserType.Customer.ToString());
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_dataContext.Managers.Any())
            {
                _dataContext.Managers.Add(new Manager { User = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckCustomerAsync(User user)
        {
            if (!_dataContext.Customers.Any())
            {
                _dataContext.Customers.Add(new Customer { User = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckDepartmentsAsync()
        {
            if (!_dataContext.Departments.Any())
            {
                _dataContext.Departments.Add(new Department
                {
                    Name = "ATLÁNTIDA",
                    CodDepHn = "01",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "LA CEIBA" , CodMunHn = "0101" },
                        new Municipality { Name = "EL PORVENIR" , CodMunHn = "0102" },
                        new Municipality { Name = "ESPARTA" , CodMunHn = "0103" },
                        new Municipality { Name = "JUTIAPA" , CodMunHn = "0104" },
                        new Municipality { Name = "LA MASICA" , CodMunHn = "0105" },
                        new Municipality { Name = "SAN FRANCISCO" , CodMunHn = "0106" },
                        new Municipality { Name = "TELA" , CodMunHn = "0107" },
                        new Municipality { Name = "ARIZONA" , CodMunHn = "0108" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "COLÓN",
                    CodDepHn = "02",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "TRUJILLO" , CodMunHn = "0201" },
                        new Municipality { Name = "BALFATE" , CodMunHn = "0202" },
                        new Municipality { Name = "IRIONA" , CodMunHn = "0203" },
                        new Municipality { Name = "LIMÓN" , CodMunHn = "0204" },
                        new Municipality { Name = "SABÁ" , CodMunHn = "0205" },
                        new Municipality { Name = "SANTA FE" , CodMunHn = "0206" },
                        new Municipality { Name = "SANTA ROSA DE AGUÁN" , CodMunHn = "0207" },
                        new Municipality { Name = "SONAGUERA" , CodMunHn = "0208" },
                        new Municipality { Name = "TOCOA" , CodMunHn = "0209" },
                        new Municipality { Name = "BONITO ORIENTAL" , CodMunHn = "0210" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "COMAYAGUA",
                    CodDepHn = "03",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "COMAYAGUA" , CodMunHn = "0301" },
                        new Municipality { Name = "AJUTERIQUE" , CodMunHn = "0302" },
                        new Municipality { Name = "EL ROSARIO" , CodMunHn = "0303" },
                        new Municipality { Name = "ESQUIAS" , CodMunHn = "0304" },
                        new Municipality { Name = "HUMUYA" , CodMunHn = "0305" },
                        new Municipality { Name = "LA LIBERTAD" , CodMunHn = "0306" },
                        new Municipality { Name = "LAMANI" , CodMunHn = "0307" },
                        new Municipality { Name = "LA TRINIDAD" , CodMunHn = "0308" },
                        new Municipality { Name = "LEJAMANÍ" , CodMunHn = "0309" },
                        new Municipality { Name = "MEAMBAR" , CodMunHn = "0310" },
                        new Municipality { Name = "MINAS DE ORO" , CodMunHn = "0311" },
                        new Municipality { Name = "OJOS DE AGUA" , CodMunHn = "0312" },
                        new Municipality { Name = "SAN JERÓNIMO" , CodMunHn = "0313" },
                        new Municipality { Name = "SAN JOSÉ DE COMAYAGUA" , CodMunHn = "0314" },
                        new Municipality { Name = "SAN JOSÉ DEL POTRERO" , CodMunHn = "0315" },
                        new Municipality { Name = "SAN LUIS" , CodMunHn = "0316" },
                        new Municipality { Name = "SAN SEBASTIAN" , CodMunHn = "0317" },
                        new Municipality { Name = "SIGUATEPEQUE" , CodMunHn = "0318" },
                        new Municipality { Name = "VILLA DE SAN ANTONIO" , CodMunHn = "0319" },
                        new Municipality { Name = "LAS LAJAS" , CodMunHn = "0320" },
                        new Municipality { Name = "TAULABE" , CodMunHn = "0321" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "COPÁN",
                    CodDepHn = "04",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "SANTA ROSA DE COPÁN" , CodMunHn = "0401" },
                        new Municipality { Name = "CABAÑAS" , CodMunHn = "0402" },
                        new Municipality { Name = "CONCEPCIÓN" , CodMunHn = "0403" },
                        new Municipality { Name = "COPÁN RUINAS" , CodMunHn = "0404" },
                        new Municipality { Name = "CORQUÍN" , CodMunHn = "0405" },
                        new Municipality { Name = "CUCUYAGUA" , CodMunHn = "0406" },
                        new Municipality { Name = "DOLORES" , CodMunHn = "0407" },
                        new Municipality { Name = "DULCE NOMBRE" , CodMunHn = "0408" },
                        new Municipality { Name = "EL PARAÍSO" , CodMunHn = "0409" },
                        new Municipality { Name = "FLORIDA" , CodMunHn = "0410" },
                        new Municipality { Name = "LA JIGUA" , CodMunHn = "0411" },
                        new Municipality { Name = "LA UNION" , CodMunHn = "0412" },
                        new Municipality { Name = "NUEVA ARCADIA" , CodMunHn = "0413" },
                        new Municipality { Name = "SAN AGUSTÍN" , CodMunHn = "0414" },
                        new Municipality { Name = "SAN ANTONIO" , CodMunHn = "0415" },
                        new Municipality { Name = "SAN JERONIMO" , CodMunHn = "0416" },
                        new Municipality { Name = "SAN JOSÉ" , CodMunHn = "0417" },
                        new Municipality { Name = "SAN JUAN DE OPOA" , CodMunHn = "0418" },
                        new Municipality { Name = "SAN NICOLAS" , CodMunHn = "0419" },
                        new Municipality { Name = "SAN PEDRO" , CodMunHn = "0420" },
                        new Municipality { Name = "SANTA RITA" , CodMunHn = "0421" },
                        new Municipality { Name = "TRINIDAD DE COPÁN" , CodMunHn = "0422" },
                        new Municipality { Name = "VERACRUZ" , CodMunHn = "0423" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "CORTÉS",
                    CodDepHn = "05",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "SAN PEDRO SULA" , CodMunHn = "0501" },
                        new Municipality { Name = "CHOLOMA" , CodMunHn = "0502" },
                        new Municipality { Name = "OMOA" , CodMunHn = "0503" },
                        new Municipality { Name = "PIMIENTA" , CodMunHn = "0504" },
                        new Municipality { Name = "POTRERILLOS" , CodMunHn = "0505" },
                        new Municipality { Name = "PUERTO CORTÉS" , CodMunHn = "0506" },
                        new Municipality { Name = "SAN ANTONIO DE CORTÉS" , CodMunHn = "0507" },
                        new Municipality { Name = "SAN FRANCISCO DE YOJOA" , CodMunHn = "0508" },
                        new Municipality { Name = "SAN MANUEL" , CodMunHn = "0509" },
                        new Municipality { Name = "SANTA CRUZ DE YOJOA" , CodMunHn = "0510" },
                        new Municipality { Name = "VILLANUEVA" , CodMunHn = "0511" },
                        new Municipality { Name = "LA LIMA" , CodMunHn = "0512" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "CHOLUTECA",
                    CodDepHn = "06",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "CHOLUTECA" , CodMunHn = "0601" },
                        new Municipality { Name = "APACILAGUA" , CodMunHn = "0602" },
                        new Municipality { Name = "CONCEPCIÓN DE MARÍA" , CodMunHn = "0603" },
                        new Municipality { Name = "DUYURE" , CodMunHn = "0604" },
                        new Municipality { Name = "EL CORPUS" , CodMunHn = "0605" },
                        new Municipality { Name = "EL TRIUNFO" , CodMunHn = "0606" },
                        new Municipality { Name = "MARCOVIA" , CodMunHn = "0607" },
                        new Municipality { Name = "MOROLICA" , CodMunHn = "0608" },
                        new Municipality { Name = "NAMASIGUE" , CodMunHn = "0609" },
                        new Municipality { Name = "OROCUINA" , CodMunHn = "0610" },
                        new Municipality { Name = "PESPIRE" , CodMunHn = "0611" },
                        new Municipality { Name = "SAN ANTONIO DE FLORES" , CodMunHn = "0612" },
                        new Municipality { Name = "SAN ISIDRO" , CodMunHn = "0613" },
                        new Municipality { Name = "SAN JOSÉ" , CodMunHn = "0614" },
                        new Municipality { Name = "SAN MARCOS DE COLON" , CodMunHn = "0615" },
                        new Municipality { Name = "SANTA ANA DE YUSGUARE" , CodMunHn = "0616" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "EL PARAÍSO",
                    CodDepHn = "07",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "YUSCARAN" , CodMunHn = "0701" },
                        new Municipality { Name = "ALAUCA" , CodMunHn = "0702" },
                        new Municipality { Name = "DANLI" , CodMunHn = "0703" },
                        new Municipality { Name = "EL PARAÍSO" , CodMunHn = "0704" },
                        new Municipality { Name = "GUINOPE" , CodMunHn = "0705" },
                        new Municipality { Name = "JACALEAPA" , CodMunHn = "0706" },
                        new Municipality { Name = "LIURE" , CodMunHn = "0707" },
                        new Municipality { Name = "MOROCELI" , CodMunHn = "0708" },
                        new Municipality { Name = "OROPOLI" , CodMunHn = "0709" },
                        new Municipality { Name = "POTRERILLOS" , CodMunHn = "0710" },
                        new Municipality { Name = "SAN ANTONIO DE FLORES" , CodMunHn = "0711" },
                        new Municipality { Name = "SAN LUCAS" , CodMunHn = "0712" },
                        new Municipality { Name = "SAN MATIAS" , CodMunHn = "0713" },
                        new Municipality { Name = "SOLEDAD" , CodMunHn = "0714" },
                        new Municipality { Name = "TEUPASENTI" , CodMunHn = "0715" },
                        new Municipality { Name = "TEXIGUAT" , CodMunHn = "0716" },
                        new Municipality { Name = "VADO ANCHO" , CodMunHn = "0717" },
                        new Municipality { Name = "YAUYUPE" , CodMunHn = "0718" },
                        new Municipality { Name = "TROJES" , CodMunHn = "0719" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "FRANCISCO MORAZÁN",
                    CodDepHn = "08",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "DISTRITO CENTRAL" , CodMunHn = "0801",
                        Courts = new List<Court>
                        {
                            new Court { Name = "JUZGADO DE LETRAS CIVIL DEL DEPARTAMENTO DE FRANCISCO MORAZÁN" },
                            new Court { Name = "JUZGADO DE PAZ CIVIL DEL MUNICIPIO DEL DISTRITO CENTRAL" },
                        }
                        },
                        new Municipality { Name = "ALUBAREN" , CodMunHn = "0802" },
                        new Municipality { Name = "CEDROS" , CodMunHn = "0803" },
                        new Municipality { Name = "CURAREN" , CodMunHn = "0804" },
                        new Municipality { Name = "EL PORVENIR" , CodMunHn = "0805" },
                        new Municipality { Name = "GUAIMACA" , CodMunHn = "0806" },
                        new Municipality { Name = "LA LIBERTAD" , CodMunHn = "0807" },
                        new Municipality { Name = "LA VENTA" , CodMunHn = "0808" },
                        new Municipality { Name = "LEPATERIQUE" , CodMunHn = "0809" },
                        new Municipality { Name = "MARAITA" , CodMunHn = "0810" },
                        new Municipality { Name = "MARALE" , CodMunHn = "0811" },
                        new Municipality { Name = "NUEVA ARMENIA" , CodMunHn = "0812" },
                        new Municipality { Name = "OJOJONA" , CodMunHn = "0813" },
                        new Municipality { Name = "ORICA" , CodMunHn = "0814" },
                        new Municipality { Name = "REITOCA" , CodMunHn = "0815" },
                        new Municipality { Name = "SABANAGRANDE" , CodMunHn = "0816" },
                        new Municipality { Name = "SAN ANTONIO DE ORIENTE" , CodMunHn = "0817" },
                        new Municipality { Name = "SAN BUENAVENTURA" , CodMunHn = "0818" },
                        new Municipality { Name = "SAN IGNACIO" , CodMunHn = "0819" },
                        new Municipality { Name = "SAN JUAN DE FLORES" , CodMunHn = "0820" },
                        new Municipality { Name = "SAN MIGUELITO" , CodMunHn = "0821" },
                        new Municipality { Name = "SANTA ANA" , CodMunHn = "0822" },
                        new Municipality { Name = "SANTA LUCIA" , CodMunHn = "0823" },
                        new Municipality { Name = "TALANGA" , CodMunHn = "0824" },
                        new Municipality { Name = "TATUMBLA" , CodMunHn = "0825" },
                        new Municipality { Name = "VALLE DE ÁNGELES" , CodMunHn = "0826" },
                        new Municipality { Name = "VILLA DE SAN FRANCISCO" , CodMunHn = "0827" },
                        new Municipality { Name = "VALLECILLO" , CodMunHn = "0828" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "GRACIAS A DIOS",
                    CodDepHn = "09",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "PUERTO LEMPIRA" , CodMunHn = "0901" },
                        new Municipality { Name = "BRUS LAGUNA" , CodMunHn = "0902" },
                        new Municipality { Name = "AHUAS" , CodMunHn = "0903" },
                        new Municipality { Name = "JUAN FRANCISCO  BULNES" , CodMunHn = "0904" },
                        new Municipality { Name = "RAMÓN VILLEDA MORALES" , CodMunHn = "0905" },
                        new Municipality { Name = "WAMPUSIRPI" , CodMunHn = "0906" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "INTIBUCÁ",
                    CodDepHn = "10",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "LA ESPERANZA" , CodMunHn = "1001" },
                        new Municipality { Name = "CAMASCA" , CodMunHn = "1002" },
                        new Municipality { Name = "COLOMONCAGUA" , CodMunHn = "1003" },
                        new Municipality { Name = "CONCEPCIÓN" , CodMunHn = "1004" },
                        new Municipality { Name = "DOLORES" , CodMunHn = "1005" },
                        new Municipality { Name = "INTIBUCA" , CodMunHn = "1006" },
                        new Municipality { Name = "JESÚS DE OTORO" , CodMunHn = "1007" },
                        new Municipality { Name = "MAGDALENA" , CodMunHn = "1008" },
                        new Municipality { Name = "MASAGUARA" , CodMunHn = "1009" },
                        new Municipality { Name = "SAN ANTONIO" , CodMunHn = "1010" },
                        new Municipality { Name = "SAN ISIDRO" , CodMunHn = "1011" },
                        new Municipality { Name = "SAN JUAN" , CodMunHn = "1012" },
                        new Municipality { Name = "SAN MARCOS DE SIERRA" , CodMunHn = "1013" },
                        new Municipality { Name = "SAN MIGUELITO" , CodMunHn = "1014" },
                        new Municipality { Name = "SANTA LUCIA" , CodMunHn = "1015" },
                        new Municipality { Name = "YAMARANGUILA" , CodMunHn = "1016" },
                        new Municipality { Name = "SAN FRANCISCO DE OPALACA" , CodMunHn = "1017" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "ISLAS DE LA BAHÍA",
                    CodDepHn = "11",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "ROATAN" , CodMunHn = "1101" },
                        new Municipality { Name = "GUANAJA" , CodMunHn = "1102" },
                        new Municipality { Name = "JOSÉ SANTOS GUARDIOLA" , CodMunHn = "1103" },
                        new Municipality { Name = "UTILA" , CodMunHn = "1104" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "LA PAZ",
                    CodDepHn = "12",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "LA PAZ" , CodMunHn = "1201" },
                        new Municipality { Name = "AGUANQUETERIQUE" , CodMunHn = "1202" },
                        new Municipality { Name = "CABANAS" , CodMunHn = "1203" },
                        new Municipality { Name = "CANE" , CodMunHn = "1204" },
                        new Municipality { Name = "CHINACLA" , CodMunHn = "1205" },
                        new Municipality { Name = "GUAJIQUIRO" , CodMunHn = "1206" },
                        new Municipality { Name = "LAUTERIQUE" , CodMunHn = "1207" },
                        new Municipality { Name = "MARCALA" , CodMunHn = "1208" },
                        new Municipality { Name = "MERCEDES DE ORIENTE" , CodMunHn = "1209" },
                        new Municipality { Name = "OPATORO" , CodMunHn = "1210" },
                        new Municipality { Name = "SAN ANTONIO DEL NORTE" , CodMunHn = "1211" },
                        new Municipality { Name = "SAN JOSÉ" , CodMunHn = "1212" },
                        new Municipality { Name = "SAN JUAN" , CodMunHn = "1213" },
                        new Municipality { Name = "SAN PEDRO DE TUTULE" , CodMunHn = "1214" },
                        new Municipality { Name = "SANTA ANA" , CodMunHn = "1215" },
                        new Municipality { Name = "SANTA ELENA" , CodMunHn = "1216" },
                        new Municipality { Name = "SANTA MARÍA" , CodMunHn = "1217" },
                        new Municipality { Name = "SANTIAGO DE PURINGLA" , CodMunHn = "1218" },
                        new Municipality { Name = "YARULA" , CodMunHn = "1219" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "LEMPIRA",
                    CodDepHn = "13",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "GRACIAS" , CodMunHn = "1301" },
                        new Municipality { Name = "BELEN" , CodMunHn = "1302" },
                        new Municipality { Name = "CANDELARIA" , CodMunHn = "1303" },
                        new Municipality { Name = "COLOLACA" , CodMunHn = "1304" },
                        new Municipality { Name = "ERANDIQUE" , CodMunHn = "1305" },
                        new Municipality { Name = "GUALCINCE" , CodMunHn = "1306" },
                        new Municipality { Name = "GUARITA" , CodMunHn = "1307" },
                        new Municipality { Name = "LA CAMPA" , CodMunHn = "1308" },
                        new Municipality { Name = "LA IGUALA" , CodMunHn = "1309" },
                        new Municipality { Name = "LAS FLORES" , CodMunHn = "1310" },
                        new Municipality { Name = "LA UNION" , CodMunHn = "1311" },
                        new Municipality { Name = "LA VIRTUD" , CodMunHn = "1312" },
                        new Municipality { Name = "LEPAERA" , CodMunHn = "1313" },
                        new Municipality { Name = "MAPULACA" , CodMunHn = "1314" },
                        new Municipality { Name = "PIRAERA" , CodMunHn = "1315" },
                        new Municipality { Name = "SAN ANDRES" , CodMunHn = "1316" },
                        new Municipality { Name = "SAN FRANCISCO" , CodMunHn = "1317" },
                        new Municipality { Name = "SAN JUAN GUARITA" , CodMunHn = "1318" },
                        new Municipality { Name = "SAN MANUEL COLOHETE" , CodMunHn = "1319" },
                        new Municipality { Name = "SAN RAFAEL" , CodMunHn = "1320" },
                        new Municipality { Name = "SAN SEBASTIAN" , CodMunHn = "1321" },
                        new Municipality { Name = "SANTA CRUZ" , CodMunHn = "1322" },
                        new Municipality { Name = "TALGUA" , CodMunHn = "1323" },
                        new Municipality { Name = "TAMBLA" , CodMunHn = "1324" },
                        new Municipality { Name = "TOMALA" , CodMunHn = "1325" },
                        new Municipality { Name = "VALLADOLID" , CodMunHn = "1326" },
                        new Municipality { Name = "VIRGINIA" , CodMunHn = "1327" },
                        new Municipality { Name = "SAN MARCOS DE CAIQUIN" , CodMunHn = "1328" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "OCOTEPEQUE",
                    CodDepHn = "14",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "OCOTEPEQUE" , CodMunHn = "1401" },
                        new Municipality { Name = "BELEN GUALCHO" , CodMunHn = "1402" },
                        new Municipality { Name = "CONCEPCIÓN" , CodMunHn = "1403" },
                        new Municipality { Name = "DOLORES MERENDON" , CodMunHn = "1404" },
                        new Municipality { Name = "FRATERNIDAD" , CodMunHn = "1405" },
                        new Municipality { Name = "LA ENCARNACION" , CodMunHn = "1406" },
                        new Municipality { Name = "LA LABOR" , CodMunHn = "1407" },
                        new Municipality { Name = "LUCERNA" , CodMunHn = "1408" },
                        new Municipality { Name = "MERCEDES" , CodMunHn = "1409" },
                        new Municipality { Name = "SAN FERNANDO" , CodMunHn = "1410" },
                        new Municipality { Name = "SAN FRANCISCO DEL VALLE" , CodMunHn = "1411" },
                        new Municipality { Name = "SAN JORGE" , CodMunHn = "1412" },
                        new Municipality { Name = "SAN MARCOS" , CodMunHn = "1413" },
                        new Municipality { Name = "SANTA FE" , CodMunHn = "1414" },
                        new Municipality { Name = "SENSENTI" , CodMunHn = "1415" },
                        new Municipality { Name = "SINUAPA" , CodMunHn = "1416" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "OLANCHO",
                    CodDepHn = "15",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "JUTICALPA" , CodMunHn = "1501" },
                        new Municipality { Name = "CAMPAMENTO" , CodMunHn = "1502" },
                        new Municipality { Name = "CATACAMAS" , CodMunHn = "1503" },
                        new Municipality { Name = "CONCORDIA" , CodMunHn = "1504" },
                        new Municipality { Name = "DULCE NOMBRE DE CULMI" , CodMunHn = "1505" },
                        new Municipality { Name = "EL ROSARIO" , CodMunHn = "1506" },
                        new Municipality { Name = "ESQUIPULAS DEL NORTE" , CodMunHn = "1507" },
                        new Municipality { Name = "GUALACO" , CodMunHn = "1508" },
                        new Municipality { Name = "GUARIZAMA" , CodMunHn = "1509" },
                        new Municipality { Name = "GUATA" , CodMunHn = "1510" },
                        new Municipality { Name = "GUAYAPE" , CodMunHn = "1511" },
                        new Municipality { Name = "JANO" , CodMunHn = "1512" },
                        new Municipality { Name = "LA UNION" , CodMunHn = "1513" },
                        new Municipality { Name = "MANGULILE" , CodMunHn = "1514" },
                        new Municipality { Name = "MANTO" , CodMunHn = "1515" },
                        new Municipality { Name = "SALAMA" , CodMunHn = "1516" },
                        new Municipality { Name = "SAN ESTEBAN" , CodMunHn = "1517" },
                        new Municipality { Name = "SAN FRANCISCO DE BECERRA" , CodMunHn = "1518" },
                        new Municipality { Name = "SAN FRANCISCO DE LA PAZ" , CodMunHn = "1519" },
                        new Municipality { Name = "SANTA MARÍA DEL REAL" , CodMunHn = "1520" },
                        new Municipality { Name = "SILCA" , CodMunHn = "1521" },
                        new Municipality { Name = "YOCON" , CodMunHn = "1522" },
                        new Municipality { Name = "PATUCA" , CodMunHn = "1523" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "SANTA BÁRBARA",
                    CodDepHn = "16",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "SANTA BARBARA" , CodMunHn = "1601" },
                        new Municipality { Name = "ARADA" , CodMunHn = "1602" },
                        new Municipality { Name = "ATIMA" , CodMunHn = "1603" },
                        new Municipality { Name = "AZACUALPA" , CodMunHn = "1604" },
                        new Municipality { Name = "CEGUACA" , CodMunHn = "1605" },
                        new Municipality { Name = "SAN JOSÉ DE COLINAS" , CodMunHn = "1606" },
                        new Municipality { Name = "CONCEPCIÓN DEL NORTE" , CodMunHn = "1607" },
                        new Municipality { Name = "CONCEPCIÓN DEL SUR" , CodMunHn = "1608" },
                        new Municipality { Name = "CHINDA" , CodMunHn = "1609" },
                        new Municipality { Name = "EL NISPERO" , CodMunHn = "1610" },
                        new Municipality { Name = "GUALALA" , CodMunHn = "1611" },
                        new Municipality { Name = "ILAMA" , CodMunHn = "1612" },
                        new Municipality { Name = "MACUELIZO" , CodMunHn = "1613" },
                        new Municipality { Name = "NARANJITO" , CodMunHn = "1614" },
                        new Municipality { Name = "NUEVA CELILAC" , CodMunHn = "1615" },
                        new Municipality { Name = "PETOA" , CodMunHn = "1616" },
                        new Municipality { Name = "PROTECCIÓN" , CodMunHn = "1617" },
                        new Municipality { Name = "QUIMISTAN" , CodMunHn = "1618" },
                        new Municipality { Name = "SAN FRANCISCO DE OJUERA" , CodMunHn = "1619" },
                        new Municipality { Name = "SAN LUIS" , CodMunHn = "1620" },
                        new Municipality { Name = "SAN MARCOS" , CodMunHn = "1621" },
                        new Municipality { Name = "SAN NICOLAS" , CodMunHn = "1622" },
                        new Municipality { Name = "SAN PEDRO ZACAPA" , CodMunHn = "1623" },
                        new Municipality { Name = "SANTA RITA" , CodMunHn = "1624" },
                        new Municipality { Name = "SAN VICENTE CENTENARIO" , CodMunHn = "1625" },
                        new Municipality { Name = "TRINIDAD" , CodMunHn = "1626" },
                        new Municipality { Name = "LAS VEGAS" , CodMunHn = "1627" },
                        new Municipality { Name = "NUEVA FRONTERA" , CodMunHn = "1628" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "VALLE",
                    CodDepHn = "17",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "NACAOME" , CodMunHn = "1701" },
                        new Municipality { Name = "ALIANZA" , CodMunHn = "1702" },
                        new Municipality { Name = "AMAPALA" , CodMunHn = "1703" },
                        new Municipality { Name = "ARAMECINA" , CodMunHn = "1704" },
                        new Municipality { Name = "CARIDAD" , CodMunHn = "1705" },
                        new Municipality { Name = "GOASCORAN" , CodMunHn = "1706" },
                        new Municipality { Name = "LANGUE" , CodMunHn = "1707" },
                        new Municipality { Name = "SAN FRANCISCO DE CORAY" , CodMunHn = "1708" },
                        new Municipality { Name = "SAN LORENZO" , CodMunHn = "1709" },
                    }
                });

                _dataContext.Departments.Add(new Department
                {
                    Name = "YORO",
                    CodDepHn = "18",
                    Municipalities = new List<Municipality>
                    {
                        new Municipality { Name = "YORO" , CodMunHn = "1801" },
                        new Municipality { Name = "ARENAL" , CodMunHn = "1802" },
                        new Municipality { Name = "EL NEGRITO" , CodMunHn = "1803" },
                        new Municipality { Name = "EL PROGRESO" , CodMunHn = "1804" },
                        new Municipality { Name = "JOCON" , CodMunHn = "1805" },
                        new Municipality { Name = "MORAZAN" , CodMunHn = "1806" },
                        new Municipality { Name = "OLANCHITO" , CodMunHn = "1807" },
                        new Municipality { Name = "SANTA RITA" , CodMunHn = "1808" },
                        new Municipality { Name = "SULACO" , CodMunHn = "1809" },
                        new Municipality { Name = "VICTORIA" , CodMunHn = "1810" },
                        new Municipality { Name = "YORITO" , CodMunHn = "1811" },
                    }
                });

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}