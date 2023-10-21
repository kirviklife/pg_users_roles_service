using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APP_PG_USERS_ROLES_SERVICE.Models
{
    public class databases
    {
        [Key]
        public Guid id_db { get; set; }
        [Display(Name = "Наименование БД")]
        [Column(TypeName = "varchar")]
        [StringLength(75, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 75 символов")]
        public string db_name { get; set; }

        [Display(Name = "Сервер БД")]
        [ForeignKey("srv_id")]
        public Guid srv_id { get; set; }
        [Display(Name = "OID объекта")]
        [Range(1, int.MaxValue, ErrorMessage = "Только положительные числа или 0")]
        public int oid_db { get; set; }
        public virtual servers servers { get; set; }
        public databases()
        {
            this.db_grants = new HashSet<db_grants>();
            this.schemas = new HashSet<schemas>();
        }
        public virtual ICollection<db_grants> db_grants { get; set; }
        public virtual ICollection<schemas> schemas { get; set; }

    }

    public class db_grant_privs
    {
        [Key]
        public Guid id_db_grant_privs { get; set; }
        [Display(Name = "Привилегии")]
        [Column(TypeName = "varchar")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 150 символов")]
        public string db_grant_priv_name { get; set; }
        public db_grant_privs()
        {
            this.db_grants = new HashSet<db_grants>();
        }
        public virtual ICollection<db_grants> db_grants { get; set; }
    }

    public class db_grants
    {
        [Key]
        public Guid id_db_grants { get; set; }
        [Display(Name = "Привилегии")]
        public Guid db_grant_privs_id { get; set; }
        [Display(Name = "База данных")]
        public Guid db_id { get; set; }
        [Display(Name = "Время выполнено")]
        [Column(TypeName = "timestamp")]
        public DateTime? date_time_exec { get; set; }
        [Display(Name = "Выполнено (да/нет)")]
        [Column(TypeName = "bool")]
        public bool is_success { get; set; }
        [Display(Name = "Роль")]
        public Guid role_id { get; set; }
        [ForeignKey("role_id")]
        public roles roles { get; set; }
        [ForeignKey("db_id")]
        public databases databases { get; set; }
        [ForeignKey("db_grant_privs_id")]
        public db_grant_privs db_grant_privs { get; set; }
    }

    public class not_typical_grants
    {
        [Key]
        public Guid id_not_typical_grant { get; set; }
        [Display(Name = "Задание для выполнения")]
        public Guid task_id { get; set; }
        [Display(Name = "Дата и время последнего выполнения")]
        [Column(TypeName = "timestamp")]
        public DateTime? last_date_time_exec { get; set; }
        [Display(Name = "Дата и время создания")]
        [Column(TypeName = "timestamp")]
        public DateTime? date_time_create { get; set; }
        [Display(Name = "Схема")]
        public Guid schm_id { get; set; }
        [Display(Name = "Повторять выполнение")]
        [Column(TypeName = "bool")]
        public bool is_repeat { get; set; }
        [Display(Name = "Параметры для скрипта")]
        [Column(TypeName = "text[]")]
        public string[] parameters { get; set; }
        [Display(Name = "Роль")]
        public Guid role_id { get; set; }
        [ForeignKey("role_id")]
        public roles roles { get; set; }
        [ForeignKey("task_id")]
        public tasks_not_typical_grants tasks_not_typical_grants { get; set; }
        [ForeignKey("schm_id")]
        public schemas schemas { get; set; }
        public not_typical_grants()
        {
            this.not_typical_grants_exec = new HashSet<not_typical_grants_exec>();
        }
        public virtual ICollection<not_typical_grants_exec> not_typical_grants_exec { get; set; }
    }

    public class not_typical_grants_exec
    {
        [Key]
        public Guid id_not_typical_grant_exec { get; set; }
        [Display(Name = "Шаг задания для выполнения")]
        public Guid not_typical_grant_id { get; set; }
        [Display(Name = "Время выполнения")]
        [Column(TypeName = "timestamp")]
        public DateTime? date_time_exec { get; set; }
        [Display(Name = "Результат выполнения")]
        [Column(TypeName = "bool")]
        public bool is_success { get; set; }
        [ForeignKey("not_typical_grant_id")]
        public not_typical_grants not_typical_grants { get; set; }
        public not_typical_grants_exec()
        {
            this.not_typical_grants_exec_log = new HashSet<not_typical_grants_exec_log>();
        }
        public virtual ICollection<not_typical_grants_exec_log> not_typical_grants_exec_log { get; set; }

    }

    public class not_typical_grants_exec_log
    {
        [Key]
        public Guid id_not_typical_grants_exec_log { get; set; }
        [Display(Name = "Код шага задания для выполнения")]
        public Guid not_typical_grant_exec_id { get; set; }
        [Display(Name = "Текст ошибки")]
        [Column(TypeName = "text")]
        public string txt_error { get; set; }
        [ForeignKey("not_typical_grant_exec_id")]
        public not_typical_grants_exec not_typical_grants_exec { get; set; }
    }

    public class roles
    {
        [Key]
        public Guid id_role { get; set; }
        [Display(Name = "Имя роли")]
        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string role_name { get; set; }
        [Display(Name = "Пароль роли")]
        [Column(TypeName = "varchar")]
        public string? role_pass { get; set; }
        [Display(Name = "Адрес электронной почты")]
        [Column(TypeName = "varchar")]
        public string? email { get; set; }
        [Display(Name = "Номер мобильного телефона")]
        [Column(TypeName = "varchar")]
        public string? phone { get; set; }
        [Display(Name = "Фамилия")]
        [Column(TypeName = "varchar")]
        public string? fam { get; set; }
        [Display(Name = "Имя")]
        [Column(TypeName = "varchar")]
        public string? im { get; set; }
        [Display(Name = "Отчество")]
        [Column(TypeName = "varchar")]
        public string? otch { get; set; }
        [Display(Name = "Обновить пароль везде?")]
        [Column(TypeName = "bool")]
        public bool is_new_password { get; set; }
        [Display(Name = "Разрешить подключение к БД?")]
        [Column(TypeName = "bool")]
        public bool is_login { get; set; }
        [Display(Name = "Выдать права суперпользователя?")]
        [Column(TypeName = "bool")]
        public bool is_superuser { get; set; }
        [Display(Name = "Выдать права на создание БД?")]
        [Column(TypeName = "bool")]
        public bool is_createdb { get; set; }
        [Display(Name = "Выдать права на создание ролей?")]
        [Column(TypeName = "bool")]
        public bool is_createrole { get; set; }
        [Display(Name = "Будет ли роль «наследовать» права ролей, членом которых она является?")]
        [Column(TypeName = "bool")]
        public bool is_inherit { get; set; }
        [Display(Name = "Будет ли роль ролью репликации?")]
        [Column(TypeName = "bool")]
        public bool is_replication { get; set; }
        [Display(Name = "Устанавить дату и время, после которого пароль роли перестаёт действовать")]
        [Column(TypeName = "timestamp")]
        public DateTime? valid_until { get; set; }
        public roles()
        {
            this.users_roles_relation1 = new HashSet<users_roles_relation>();
            this.users_roles_relation2 = new HashSet<users_roles_relation>();
            this.db_grants = new HashSet<db_grants>();
            this.srv_roles_relations = new HashSet<srv_roles_relations>();
            this.not_typical_grants = new HashSet<not_typical_grants>();

		}
        public virtual ICollection<srv_roles_relations> srv_roles_relations { get; set; }
        public virtual ICollection<users_roles_relation> users_roles_relation1 { get; set; }
        public virtual ICollection<users_roles_relation> users_roles_relation2 { get; set; }
        public virtual ICollection<db_grants> db_grants { get; set; }
		public virtual ICollection<not_typical_grants> not_typical_grants { get; set; }
	}

    public class schemas
    {
        [Key]
        public Guid id_schm { get; set; }
        [Display(Name = "Наименование схемы")]
        [Column(TypeName = "varchar")]
        [StringLength(75, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 75 символов")]
        public string schm_name { get; set; }
        [Display(Name = "Наименование БД")]
        public Guid db_id { get; set; }
        [Display(Name = "OID объекта")]
        [Column(TypeName = "int4")]
        public int oid_schm { get; set; }
        [ForeignKey("db_id")]
        public databases databases { get; set; }
        public schemas()
        {
            this.not_typical_grants = new HashSet<not_typical_grants>();
            this.schm_grants = new HashSet<schm_grants>();

        }
        public virtual ICollection<not_typical_grants> not_typical_grants { get; set; }
        public virtual ICollection<schm_grants> schm_grants { get; set; }

    }

    public class schm_grant_privs
    {
        [Key]
        public Guid id_schm_grant_privs { get; set; }
        [Display(Name = "Привилегии")]
        [Column(TypeName = "varchar")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 150 символов")]
        public string schm_grant_priv_name { get; set; }
        public schm_grant_privs()
        {
            this.schm_grants = new HashSet<schm_grants>();
        }
        public virtual ICollection<schm_grants> schm_grants { get; set; }
    }

    public class schm_grants
    {
        [Key]
        public Guid id_schm_grants { get; set; }
        [Display(Name = "Привилегии")]
        public Guid schm_grant_privs_id { get; set; }
        [Display(Name = "Схема")]
        public Guid schm_id { get; set; }
        [Column(TypeName = "timestamp")]
        [Display(Name = "Дата и время выполнения")]
        public DateTime? date_time_exec { get; set; }
        [Display(Name = "Выполнено (да/нет)")]
        [Column(TypeName = "bool")]
        public bool is_success { get; set; }
        [Display(Name = "Роль")]
        public Guid role_id { get; set; }
        [ForeignKey("role_id")]
        public roles roles { get; set; }
        [ForeignKey("schm_grant_privs_id")]
        public schm_grant_privs schm_grant_privs { get; set; }
        [ForeignKey("schm_id")]
        public schemas schemas { get; set; }
    }

    public class servers
    {
        [Key]
        [Column(TypeName = "uuid")]
        public Guid id_srv { get; set; }
        [Display(Name = "IP-адрес")]
        [Column(TypeName = "varchar")]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", ErrorMessage = "Укажите ip по формату: 000.000.000.000")]
        public string ipadd { get; set; }
        [Display(Name = "Имя сервера")]
        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Не указано имя сервера")]
        [StringLength(75, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 75 символов")]
        public string srv_name { get; set; }
        [Display(Name = "Порт")]
        [Column(TypeName = "int4")]
        public int port_srv { get; set; }
        [Display(Name = "Имя пользователя для подключения по DBLink")]
        [Column(TypeName = "varchar")]
        [StringLength(75, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 75 символов")]
        public string username { get; set; }

        public servers()
        {
            this.databases = new HashSet<databases>();
            this.srv_roles_relations = new HashSet<srv_roles_relations>();
        }
        public virtual ICollection<srv_roles_relations> srv_roles_relations { get; set; }
        public virtual ICollection<databases> databases { get; set; }
    }

    public class srv_roles_relations
    {
        [Key]
        [Column(TypeName = "uuid")]
        public Guid id_srv_role { get; set; }
        [Display(Name = "OID объекта")]
        [Range(1, int.MaxValue, ErrorMessage = "Только положительные числа или 0")]
        public int? oid_roles { get; set; }
        [Display(Name = "Сервер БД")]
        public Guid srv_id { get; set; }
		[ForeignKey("srv_id")]
		[JsonIgnore]
		public virtual servers servers { get; set; }
        [Display(Name = "Роль")]
        
        public Guid role_id { get; set; }
		[ForeignKey("role_id")]
		[JsonIgnore]
		public virtual roles roles { get; set; }
    }

    [Table("view_servers_connect_checks")]
    public class view_servers_connect_checks
    {
        [Key]
        [Column(TypeName = "uuid")]
        public Guid id_srv { get; set; }
        [Display(Name = "IP-адрес")]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", ErrorMessage = "Укажите ip по формату: 000.000.000.000")]
        public string ipadd { get; set; }
        [Display(Name = "Имя сервера")]
        [StringLength(75, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 75 символов")]
        public string srv_name { get; set; }
        [Display(Name = "Порт")]
        public int port_srv { get; set; }
        [Display(Name = "Имя пользователя для подключения по DBLink")]
        public string username { get; set; }
        [Display(Name = "Проверка коннекта")]
        public string check { get; set; }
    }

    public class tasks_not_typical_grants
    {
        [Key]
        public Guid id_task { get; set; }
        [Display(Name = "Наименование задания")]
        [Column(TypeName = "varchar")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 150 символов")]
        public string task_name { get; set; }
        [Display(Name = "Описание")]
        [Column(TypeName = "varchar")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 300 символов")]
        public string task_descr { get; set; }
        [Display(Name = "Скрипт для выполнения")]
        [Column(TypeName = "text")]
        public string task_script { get; set; }
        [Display(Name = "Кол-во параметров в скрипте")]
        [Column(TypeName = "int4")]
        [Range(0, int.MaxValue, ErrorMessage = "Только положительные числа или 0")]
        public int count_params { get; set; }
        public tasks_not_typical_grants()
        {
            this.not_typical_grants = new HashSet<not_typical_grants>();
        }
        public virtual ICollection<not_typical_grants> not_typical_grants { get; set; }

    }

    public class users_roles_relation
    {
        [Key]
        public Guid id_users_roles_relation { get; set; }

        [Display(Name = "Роль1 (которую назначаем Роли2)")]
        public Guid from_role { get; set; }
        [Display(Name = "Роль2 (которой назначаем Роль1)")]
        public Guid to_role { get; set; }
        [Column(TypeName = "timestamp")]
        [Display(Name = "Дата и время выполнения")]
        public DateTime? date_time_excec { get; set; }
        [ForeignKey("from_role")]
        public roles roles1 { get; set; }
        [ForeignKey("to_role")]
        public roles roles2 { get; set; }
    }

    public class SrvData
    {
        public IEnumerable<databases> databases { get; set; }
        public IEnumerable<srv_roles_relations> srv_roles_relations { get; set; }
        public IEnumerable<users_roles_relation> users_roles_relation { get; set; }
        public IEnumerable<v_users_roles_grants> v_users_roles_grants { get; set; }
    }

    public class DBData
    {
        public IEnumerable<databases> databases { get; set; }
        public IEnumerable<schemas> schemas { get; set; }
        public IEnumerable<db_grants> db_grants { get; set; }
        public IEnumerable<db_grant_privs> db_grant_privs { get; set; }
        public IEnumerable<tasks_not_typical_grants> tasks_not_typical_grants { get; set; }
        public IEnumerable<not_typical_grants> not_typical_grants { get; set; }
        public IEnumerable<schm_grants> schm_grants { get; set; }
        public IEnumerable<schm_grant_privs> schm_grant_privs { get; set; }
    }

    [Table("v_users_roles_grants")]
    [Keyless]
    public class v_users_roles_grants
    {
        [Display(Name = "Пользователь/Роль")]
        public string member_name { get; set; }
        [Display(Name = "Код пользователя/роли")]
        public int member { get; set; }
        [Display(Name = "Присвоенная роль")]
        public string role { get; set; }
        [Display(Name = "Код присовенной роли")]
        public int roleid { get; set; }
        [Display(Name = "Путь")]
        public string path { get; set; }

    }


    public class StatServers
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
    }
	public class StatRoles
	{
		public string Title { get; set; }
		public int Quantity { get; set; }
	}

	public class Statjobs
	{
		public string Title { get; set; }
		public int Quantity { get; set; }
	}

	public class StackedViewModel
    {
        public List<StatServers> StatServers { get; set; }
        public List<StatRoles> StatRoles { get; set; }
        public List<jobs_status> jobs_status { get; set; }
        public List<grants_status> grants_status { get; set; }
	}

	[Table("jobs_status")]
	[Keyless]
	public class jobs_status
	{
		[Display(Name = "Дата")]
		public string my_day { get; set; }
		[Display(Name = "Статус")]
		public string status { get; set; }
		[Display(Name = "Количество")]
		public int coalesce { get; set; }

	}

	[Keyless]
	[Table("grants_status")]
	public class grants_status
	{
		public DateOnly data { get; set; }
		public string znach { get; set; }
		public int coalesce { get; set; }

	}

	[Keyless]
	[Table("view_log_not_typical_grants")]
	public class view_log_not_typical_grants
	{
		public string srv_name { get; set; }
		public string ipadd { get; set; }
		public string db_name { get; set; }
		public string schm_name { get; set; }
		public DateTime date_time_exec { get; set; }
		public bool is_success { get; set; }
		public string task_name { get; set; }
		public string zadanie { get; set; }
		public string? txt_error { get; set; }

	}

}

