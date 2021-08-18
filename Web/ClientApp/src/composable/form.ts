import { reactive, UnwrapRef } from '@vue/composition-api';
import { Dictionary } from '@/services/model';

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
