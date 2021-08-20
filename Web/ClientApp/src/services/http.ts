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
import { Dictionary, Page, PageMeta, Primitive } from '@/services/model';

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
  params?: Partial<Q>,
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

  const baseQuery = { page: 1, size: 15, ...params };
  if (options.syncQuery) {
    Object.assign(baseQuery, mergeQuery(route.query, baseQuery, params, options.syncFields));
  }
  const query = reactive<PagedQueryParams<Q>>(baseQuery as PagedQueryParams<Q>);

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
        await router.push({ query: (newParams as any) }).catch(noop);
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
 * @param fields field to include, if not specified, merge all field
 */
function mergeQuery(origin: Dictionary, params: Dictionary, defaultValues: Dictionary = {}, fields?: string[]): Dictionary {
  if (!params) return origin;

  Object.keys(params).forEach((param) => {
    // Check field if sync fields specified
    if (fields && !fields.includes(param)) {
      return;
    }

    const value = params[param];
    if (typeof value === 'object') {
      return;
    }
    const parsedValue = convertParamValue(params[param] as Primitive | Primitive[], origin[param], defaultValues[param]);
    origin[param] = parsedValue;
    if (parsedValue === null) {
      delete origin[param];
    }
  });

  return origin;
}

/**
 * Try to restore value type from example
 * ex:
 * value = '2', example = 1 => origin = 2
 * value = 2, example = 1 => origin = 2
 */
function convertParamValue<T>(raw: Primitive | Primitive[], example?: T, defaultValue?: T): T | null {
  if (raw === null || raw === undefined) {
    return defaultValue ?? null;
  }

  if (example === null || example === undefined) {
    return raw as unknown as T; // unknown type of T as T is null
  }

  if (Array.isArray(raw)) {
    if (Array.isArray(example)) {
      return raw as unknown as T;
    }
    // If current param type is not array, then process first element
    if (raw.length === 0) raw.push('');
    [raw] = raw;
  } else if (typeof raw === typeof example) {
    return raw as unknown as T;
  }

  // Convert if type is different
  const value = raw?.toString().trim();
  if (!value) {
    return defaultValue ?? null;
  }

  if (typeof example === 'string') {
    return value as unknown as T;
  }

  if (typeof example === 'boolean') {
    return (value === 'true') as unknown as T;
  }

  if (typeof example === 'number') {
    const num = Number(value);
    if (!Number.isNaN(num)) {
      return num as unknown as T;
    }
  }
  return null;
}
