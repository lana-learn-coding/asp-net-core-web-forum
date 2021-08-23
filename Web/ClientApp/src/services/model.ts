export type Primitive = string | number | boolean

export interface Dictionary {
  [key: string]: Primitive | Primitive[] | Dictionary | Dictionary[];
}

export interface FlatDictionary {
  [key: string]: Primitive | Primitive[];
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
  id: string;
  slug: string;
  createdAt: string;
  updatedAt: string;
}

export interface Category extends ModelBase {
  name: string;
  icon?: string;
}

export interface Tag extends ModelBase {
  name: string;
}
