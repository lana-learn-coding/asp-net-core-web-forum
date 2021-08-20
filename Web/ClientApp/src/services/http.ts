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
import { Dictionary, FlatDictionary, Page, PageMeta, Primitive } from '@/services/model';

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

export interface UseQueryResult<T, Q extends FlatDictionary> {
  data: Ref<UnwrapRef<T[]>>;
  meta: UnwrapRef<PageMeta & { loading: boolean; initialized: boolean }>;
  query: UnwrapRef<PagedQueryParam<Q>>;

  fetch(): Promise<void>;
}

type PagedQueryParam<Q> = Q & { page: number; size: number }
type RouteQueryParam = string | (string | null)[] | null | undefined
type UseQueryCurlyFunction<T> = <Q extends FlatDictionary>(params?: Q) => UseQueryResult<T, Q>

export function useQuery<T>(url: string): UseQueryCurlyFunction<T> {
  return <Q extends FlatDictionary>(params?: Q) => {
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
    const query = reactive<PagedQueryParam<Q>>(mergeQuery(baseQuery, route.query) as PagedQueryParam<Q>);

    watch(
      query,
      (val) => fetch(val as Q),
      { deep: true },
    );

    const router = useRouter();
    const client = useHttp();

    async function fetch(queryParams?: Q) {
      meta.loading = true;
      try {
        const newParams = { ...query, ...queryParams };
        const res = await client.get<Page<T>>(url, { params: newParams });

        data.value = res.data as UnwrapRefSimple<T>[];
        Object.assign(meta, res.meta);
        query.page = res.meta.currentPage;
        query.size = res.meta.perPage;

        await router.push({ query: newParams as Record<string, RouteQueryParam> }).catch(noop);
      } finally {
        meta.loading = false;
      }
    }

    onMounted(async () => {
      await fetch();
      meta.initialized = true;
    });

    return {
      data,
      meta,
      query,
      fetch,
    };
  };
}

/**
 * Merge 2 query params to new ones
 * Work like Object.assign but with some improvement that help converting route queries
 * to typed query params
 *
 * @param origin origin params. this object maintain the origin type of each param
 * @param params the params to merge
 */
function mergeQuery(origin: FlatDictionary, params: Record<string, RouteQueryParam>): FlatDictionary {
  const result = { ...origin };
  if (!params) return result;

  Object.keys(params).forEach((param) => {
    const value = convertParamValue(params[param], result[param]);
    if (value !== null) {
      result[param] = value;
    } else {
      delete result[param];
    }
  });

  return result;
}

type PrimitiveArray = Primitive | Primitive[]

/**
 * Try to restore value type from example
 * ex:
 * value = '2', example = 1 => origin = 2
 * value = 2, example = 1 => origin = 2
 */
function convertParamValue<T extends PrimitiveArray>(raw: RouteQueryParam, example?: T): T | null {
  if (raw === null || raw === undefined) {
    return null;
  }

  if (example === null || example === undefined) {
    return raw as unknown as T; // unknown type of T as T is null
  }

  if (raw instanceof Array) {
    if (Array.isArray(example)) {
      if (example.length === 0) {
        return raw as unknown as T;
      }

      // If both is an array, then parse all the array using type of first item in example
      return raw.map((val) => convertParamValue(val, example[0])) as unknown as T;
    }

    // If example is not an array, then process first raw param
    if (raw.length === 0) raw.push('');
    return convertParamValue(raw[0], example);
  }

  // Convert if type is same
  if (typeof raw === typeof example) {
    return raw as unknown as T;
  }

  // Convert if type is different
  const value = raw?.toString().trim();
  if (!value) {
    return null;
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
