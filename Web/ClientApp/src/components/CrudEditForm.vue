<template>
  <v-card :width="width">
    <v-card-title v-if="isEdit">Edit {{ title }}}</v-card-title>
    <v-card-subtitle v-if="isEdit">id: {{ slug }}</v-card-subtitle>
    <v-card-title v-else>Create {{ title }}</v-card-title>
    <v-card-text>
      <form>
        <!-- Form fields-->
        <template v-for="(_, field) of formField">
          <slot
            :name="`field.${field}`"
            :value="formField[field]"
            :error="errors[field]"
            :input="(x) => formField[field] = x"
          >
          </slot>
        </template>

        <div class="mt-3">
          <v-btn
            class="mr-4"
            @click="submit"
            color="primary"
            :loading="loading"
          >
            Submit
          </v-btn>
          <v-btn
            class="mr-4"
            @click="clear"
            color="green"
            text
          >
            Clear
          </v-btn>
          <v-btn @click="close" text>
            Back
          </v-btn>
        </div>
      </form>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { computed, defineComponent, ref, watch } from '@vue/composition-api';
import { useHttp } from '@/services/http';
import { useForm } from '@/composable/form';

export default defineComponent({
  name: 'CrudEditForm',
  props: {
    url: {
      type: String,
      required: true,
    },
    form: {
      type: Object,
      required: true,
    },
    width: {
      type: String,
      required: false,
      default: 'auto',
    },
    title: String,
    value: {
      type: String,
    },
  },
  setup(props, { emit }) {
    const slug = computed({
      get: () => props.value,
      set: (val) => emit('input', val),
    });

    const {
      form: formField,
      errors,
      setErrors,
      clearErrors,
      clearForm,
    } = useForm({ ...props.form });

    const isEdit = computed(() => !!slug.value);
    const loading = ref(false);
    const http = useHttp();

    async function submit() {
      loading.value = true;
      try {
        if (isEdit.value) {
          const data = await http.put(`${props.url}/${slug.value}`, formField);
          Object.assign(formField, data);
          emit('change');
          return;
        }
        await http.post(props.url, formField);
        emit('change');
        close();
      } catch (e) {
        setErrors(e.response.data);
      } finally {
        loading.value = false;
      }
    }

    function clear() {
      clearErrors();
      clearForm();
    }

    function close() {
      clear();
      emit('close');
    }

    watch(isEdit, async () => {
      if (isEdit.value) {
        try {
          loading.value = true;
          const data = await http.get(`/${props.url}/${slug.value}`);
          Object.assign(formField, data);
        } catch {
          close();
        } finally {
          loading.value = false;
        }
      }
    }, { immediate: true });

    return {
      formField,
      slug,
      errors,
      isEdit,
      submit,
      close,
      clear,
      loading,
    };
  },
});
</script>
