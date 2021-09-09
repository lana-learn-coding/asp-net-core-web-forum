import Vue from 'vue';

export interface BreadcrumbItem {
  name?: string,
  text: string,
  disabled?: boolean,
  extract?: boolean,
}

export const breadcrumbs = Vue.observable<{ value: BreadcrumbItem[] }>({
  value: [],
});

export function useBreadcrumbs(items: BreadcrumbItem[]): { value: BreadcrumbItem[] } {
  breadcrumbs.value = items;
  return breadcrumbs;
}
