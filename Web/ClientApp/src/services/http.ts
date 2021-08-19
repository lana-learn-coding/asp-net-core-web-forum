import axios, { AxiosInstance, AxiosPromise, AxiosRequestConfig } from 'axios';
import {
  onMounted,
  reactive,
  Ref,
  ref,
  UnwrapRef,
  UnwrapRefSimple,
  watch,
} from '@vue/composition-api';
import { noop, useRoute, useRouter } from '@/composable/compat';
import { Dictionary, Page, PageMeta } from '@/services/model';

const http = axios.create({
  baseURL: process.env.VUE_APP_API_URI || '/api',
  withCredentials: true,
});

export interface HttpClient {
  unwrapped: AxiosInstance,

  request<T>(config: AxiosRequestConfig): Promise<T>;

  get<T>(url: string, config?: AxiosRequestConfig): Promise<T>;

  delete<T>(url: string, config?: AxiosRequestConfig): Promise<T>;

  post<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T>;

  put<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T>;
}

export function useHttp(): HttpClient {
  const router = useRouter();

  async function request<T>(promise: Promise<AxiosPromise<T>>): Promise<T> {
    try {
      return (await promise).data;
    } catch (e) {
      if (e.response.status === 401) {
        await router.push({ name: 'Unauthorized' });
      }
      throw e;
    }
  }

  return {
    unwrapped: http,
    async request<T>(config: AxiosRequestConfig): Promise<T> {
      return request(http.request(config));
    },
    async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
      return request(http.get(url, config));
    },
    async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
      return request(http.delete(url, config));
    },
    async post<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T> {
      return request(http.post(url, data, config));
    },
    async put<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T> {
      return request(http.put(url, data, config));
    },
  };
}

export type PagedQueryParams<Q extends Dictionary> = Q & { page: number; size: number }

export interface UseQueryResult<T, Q extends Dictionary> {
  data: Ref<UnwrapRef<T[]>>;
  meta: UnwrapRef<PageMeta & { loading: boolean; initialized: boolean }>;
  query: UnwrapRef<PagedQueryParams<Q>>;

  fetchData(): Promise<void>;

  fetchData(): Promise<void>;
}

export interface UseQueryOptions {
  syncQuery: boolean;
  syncFields?: string[];
}

export function useQuery<T, Q extends Dictionary>(
  url: string,
  params?: Q,
  options: UseQueryOptions = { syncQuery: true },
): UseQueryResult<T, Q> {
  const data = ref<T[]>([]);
  const route = useRoute();

  const meta = reactive(
    {
      currentPage: 1,
      perPage: 15,
      totalPages: 1,
      totalItems: 0,
      loading: false,
      initialized: false,
    },
  );

  const query = reactive<PagedQueryParams<Q>>({
    page: 1,
    size: 15,
    ...params,
  } as PagedQueryParams<Q>);
  if (options.syncQuery) {
    Object.assign(query, mergeQuery(route.query, query, params, options.syncFields));
  }

  watch(
    query,
    (val) => fetchData(val as PagedQueryParams<Q>),
    { deep: true },
  );

  const router = useRouter();
  const client = useHttp();

  async function fetchData(queryParams?: PagedQueryParams<Q>) {
    meta.loading = true;
    try {
      const newParams = { ...query, ...queryParams };
      const res = await client.get<Page<T>>(url, { params: newParams });

      data.value = res.data as UnwrapRefSimple<T>[];
      Object.assign(meta, res.meta);
      query.page = res.meta.currentPage;
      query.size = res.meta.perPage;

      if (options.syncQuery) {
        await router.push({ query: newParams }).catch(noop);
      }
    } finally {
      meta.loading = false;
    }
  }

  onMounted(async () => {
    await fetchData();
    meta.initialized = true;
  });

  return {
    data,
    meta,
    query,
    fetchData,
  };
}

/**
 * Merge 2 query params to new ones
 * Work like Object.assign but with some improvement that help converting queries value
 *
 * @param origin origin params. this object maintain the origin type of each param
 * @param params the params to merge
 * @param defaultValues default values (in case the params is null, default will set origin field to empty string. use
 * this object to set default value to those field
 * @param fields field to incluce, if not specified, merge all field
 */
function mergeQuery(params: Dictionary, origin: Dictionary, defaultValues: Dictionary = {}, fields?: string[]): Dictionary {
  const currentParams = { ...origin } as Dictionary;
  if (!params) return currentParams;

  Object.keys(params).forEach((param) => {
    // Check field if sync fields specified
    if (fields && !fields.includes(param)) {
      return;
    }

    const currentValue = currentParams[param];

    // Assign directly if type is same
    const raw = params[param];
    if (raw === null || raw === undefined) {
      currentParams[param] = defaultValues[param] ?? '';
      return;
    }
    if (currentValue === null || currentValue === undefined) {
      currentParams[param] = raw;
      return;
    }
    if (Array.isArray(raw)) {
      if (Array.isArray(currentValue)) {
        currentParams[param] = raw;
        return;
      }
      // If current param type is not array, then process first element
      if (raw.length === 0) raw.push('');
      [params[param]] = raw;
    } else if (typeof raw === typeof currentValue) {
      currentParams[param] = raw;
      return;
    }

    // Convert if type is different
    const value = params[param]?.toString();
    if (!value) {
      currentParams[param] = defaultValues[param] ?? '';
      return;
    }
    if (typeof currentValue === 'boolean') {
      currentParams[param] = value === 'true';
      return;
    }
    if (typeof currentValue === 'number') {
      const num = Number(value);
      if (!Number.isNaN(num)) {
        currentParams[param] = num;
        return;
      }
    }
    currentParams[param] = value;
  });
  return currentParams;
}
