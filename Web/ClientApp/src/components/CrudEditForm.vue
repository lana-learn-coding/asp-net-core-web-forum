<template>
  <v-card
    :width="width"
    :max-width="$vuetify.breakpoint.lgAndUp ? '1200px' : $vuetify.breakpoint.width - 40"
    :min-width="$vuetify.breakpoint.mdAndUp ? '500px' : '320px'"
  >
    <v-card-title v-if="isEdit">Edit {{ formTitle }}}</v-card-title>
    <v-card-subtitle v-if="isEdit">id: {{ slug }}</v-card-subtitle>
    <v-card-title v-else>Create {{ formTitle }}</v-card-title>
    <v-card-text>
      <v-form ref="formRef">
        <!-- Form fields-->
        <template v-for="(_, field) of formField">
          <slot
            :name="`form.${field}`"
            :isEdit="isEdit"
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
            v-if="$vuetify.breakpoint.mdAndUp"
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
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { computed, defineComponent, ref, watch } from '@vue/composition-api';
import { singular } from 'pluralize';
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
    const formRef = ref(null);

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
    const formTitle = computed(() => singular(props.title || 'item'));

    async function submit() {
      if (!formRef.value.validate()) {
        return;
      }
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
      formRef.value.resetValidation();
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
      formRef,
      formField,
      slug,
      errors,
      isEdit,
      submit,
      close,
      clear,
      loading,
      formTitle,
    };
  },
});
</script>
