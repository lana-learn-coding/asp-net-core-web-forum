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
}

export interface UserBase extends ModelBase {
  avatar: string;
  username: string;
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
  title: string;
  content: string;
  user: UserBase,
  forum: Forum,
  lastActivityAt?: string;
  postsCount: number;
  tags: Tag[],
  viewsCount: number;
}

export interface Post extends ModelBase {
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
