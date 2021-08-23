import { reactive, Ref, ref, UnwrapRef } from '@vue/composition-api';
import { Category, Dictionary } from '@/services/model';
import { useHttp } from '@/services/http';

export interface UseFormResult<T> {
  form: UnwrapRef<T>;
  errors: UnwrapRef<T>;

  clearForm();

  clearErrors();

  setErrors(errors: Dictionary);

  setForm(form: Dictionary);
}

export function useForm<T extends Dictionary>(data: T): UseFormResult<T> {
  const form = reactive<T>({ ...data });
  const errors = reactive<T>({ ...data });

  function clearForm() {
    Object.assign(form, data);
  }

  function clearErrors() {
    Object.keys(data).forEach((key) => {
      (errors as Dictionary)[key] = [];
    });
  }

  function setErrors(newErrors: Dictionary) {
    clearErrors();
    Object.assign(errors, newErrors);
  }

  function setForm(newForm: Dictionary) {
    clearForm();
    Object.assign(form, newForm);
  }

  clearErrors();
  return {
    form,
    errors,
    clearForm,
    clearErrors,
    setErrors,
    setForm,
  };
}

export interface UseListResult<T> {
  data: Ref<T[]>;

  fetch(): Promise<void>;

  loading: Ref<boolean>;
}

let categoriesCache: Category[] = [];

export function useCategories(): UseListResult<Category> {
  const data = ref([...categoriesCache]);
  const loading = ref(false);
  const client = useHttp();

  async function fetch() {
    try {
      loading.value = true;
      const res = await client.get<Category[]>('/categories/all');
      categoriesCache = res;
      data.value = [...res];
    } finally {
      loading.value = false;
    }
  }

  if (categoriesCache.length === 0) {
    fetch();
  }

  return {
    data,
    fetch,
    loading,
  };
}
