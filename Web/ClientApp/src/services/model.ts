export interface Dictionary {
  [key: string]: any;
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
  createdAt: string;
  updatedAt: string;
}
