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
import { useRoute, useRouter } from '@/composable/compat';
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

  function mergeQuery(params?: Dictionary): Dictionary {
    const currentParams = { ...query } as Dictionary;
    if (!params) return currentParams;

    Object.keys(params).forEach((param) => {
      // Check field if syncFields specified
      if (options.syncFields && !options.syncFields.includes(param)) {
        return;
      }

      const currentValue = currentParams[param];

      // Assign directly if type is same
      const raw = params[param];
      if (raw === null || raw === undefined) {
        currentParams[param] = '';
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
        currentParams[param] = '';
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
      const newParams = options.syncQuery
        ? mergeQuery(queryParams) as PagedQueryParams<Q>
        : { ...query, ...queryParams };
      const res = await client.get<Page<T>>(url, { params: newParams });

      data.value = res.data as UnwrapRefSimple<T>[];
      Object.assign(meta, res.meta);
      query.page = res.meta.currentPage;
      query.size = res.meta.perPage;

      if (options.syncQuery) {
        await router.push({ query: newParams });
      }
    } finally {
      meta.loading = false;
    }
  }

  onMounted(async () => {
    if (options.syncQuery) {
      await fetchData(route.query as PagedQueryParams<Q>);
    } else {
      await fetchData();
    }
    meta.initialized = true;
  });

  return {
    data,
    meta,
    query,
    fetchData,
  };
}
