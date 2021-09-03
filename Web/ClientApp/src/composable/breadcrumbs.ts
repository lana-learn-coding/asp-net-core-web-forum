import Vue from 'vue';

export interface BreadcrumbItem {
  name?: string,
  text: string,
  disabled?: boolean,
  extract?: boolean,
}

export const breadcrumbs = Vue.observable<{ items: BreadcrumbItem[] }>({
  items: [],
});

export function useBreadcrumbs(items: BreadcrumbItem[]): { items: BreadcrumbItem[] } {
  breadcrumbs.items = items;
  return breadcrumbs;
}
