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

export type PagedQueryParams = Dictionary & { page: number; size: number }

export interface UseQueryResult<T> {
  data: Ref<UnwrapRef<T[]>>;
  meta: UnwrapRef<PageMeta & { loading: boolean; initialized: boolean }>;
  query: UnwrapRef<PagedQueryParams>;

  fetchData(): Promise<void>;
}

export interface UseQueryOptions {
  syncQuery: boolean;
}

export function useQuery<T>(
  url: string,
  params: Dictionary & { page?: number; size?: number } = {},
  options: UseQueryOptions = { syncQuery: false },
): UseQueryResult<T> {
  const route = useRoute();
  const data = ref<T[]>([]);
  const query = reactive<PagedQueryParams>({
    page: parseInt(route.params?.page ?? 1, 10),
    size: parseInt(route.params?.size ?? 15, 10),
    ...params,
  });

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

  watch(
    query,
    (val) => fetchData(val),
    { deep: true },
  );

  const router = useRouter();
  const client = useHttp();

  async function fetchData(queryParams: Dictionary & { page?: number; size?: number } = {}) {
    meta.loading = true;
    try {
      const newParams: PagedQueryParams = { ...query, ...queryParams };
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
