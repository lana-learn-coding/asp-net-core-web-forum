export type Primitive = string | number | boolean

export interface Dictionary {
  [key: string]: Primitive | Primitive[] | Dictionary | Dictionary[];
}

export interface FlatDictionary {
  [key: string]: Primitive | Primitive[];
}

export interface SingularDictionary<T> {
  [key: string]: T;
}

export interface PageMeta {
  currentPage: number;
  perPage: number;
  totalPages: number;
  totalItems: number;
}

export interface Page<T> {
  data: T[];
  meta: PageMeta;
}

export interface ModelBase {
  uid: string;
  slug: string;
  createdAt: string;
  updatedAt: string;
}

export interface Category extends ModelBase {
  name: string;
  icon?: string;
  description?: string;
  priority: number;
}

export interface Tag extends ModelBase {
  name: string;
  color?: string;
  isSpecialty: boolean;
}

export interface Specialty extends ModelBase {
  name: string;
}

export interface UserBase extends ModelBase {
  avatar: string;
  username: string;
  email: string;
}

export interface Forum extends ModelBase {
  title: string;
  icon?: string;
  subTitle: string;
  description?: string;
  threadAccess: number | string;
  forumAccess: number | string;
  categoryId: number;
  category: Category;
  priority: number;
  threadsCount: number;
  postsCount: number;
  viewsCount: number;
  lastThread: ModelBase & {
    user: UserBase,
    title: string,
    lastActivityAt: string,
  };
}

export interface Thread extends ModelBase {
  vote: number;
  voted: number;
  title: string;
  content: string;
  user: UserBase,
  forum: Forum,
  lastActivityAt?: string;
  postsCount: number;
  tags: Tag[],
  viewsCount: number;
  status: number;
}

export interface Me extends UserBase {
  firstName: string;
  lastName: string;
  gender: boolean;
  bio: string;
  phone: string;
  dateOfBirth: string;
  showPhone: boolean,
  showEmail: boolean,

  showWorkAddress: boolean,
  showWorkExperience: boolean,
  showWorkSpecialities: boolean,
  workSpecialitiesIds: string[],
  workPhone: string;
  workCountryId: string;
  workCityId: string;
  workAddress: string;
  workDescription: string;
  workExperienceId: string;
  workPositionId: string;
  createdAt: string;
}

// public override string Avatar => User.Avatar;
// public override string Username => User.Username;
// public override string Email => User.Email;
//
// public bool? Gender { get; set; }
//
// public string FirstName { get; set; }
//
// public string LastName { get; set; }
//
// public string Phone { get; set; }
//
// public string Bio { get; set; }
//
// public DateTime? DateOfBirth { get; set; }
//
// public string WorkPhone { get; set; }
//
// public string WorkAddress { get; set; }
//
// public Guid? WorkCityId { get; set; }
// public City WorkCity { get; set; }
//
// public Guid? WorkCountryId { get; set; }
// public Country WorkCountry { get; set; }
//
// public Guid? WorkPositionId { get; set; }
// public Position WorkPosition { get; set; }
//
// public Guid? WorkExperienceId { get; set; }
// public Experience WorkExperience { get; set; }
//
// public string WorkDescription { get; set; }
// public virtual ICollection<Specialty> WorkSpecialities { get; set; }
//
// public bool ShowPhone { get; set; } = false;
// public bool ShowEmail { get; set; } = true;
// public bool ShowWorkAddress { get; set; } = false;
// public bool ShowWorkExperience { get; set; } = false;
// public bool ShowWorkSpecialities { get; set; } = false;

export interface Post extends ModelBase {
  vote: number;
  voted: number;
  content: string;
  user: UserBase;
  threadTitle: string;
  threadId: string;
}

export enum AccessMode {
  Public = 0,
  Protected = 1,
  Internal = 2,
  Private = 3
}
