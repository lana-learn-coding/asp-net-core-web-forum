<template>
  <v-select
    :value="value"
    @input="input"
    :items="data"
    :loading="loading"
    :error-messages="errorMessages"
    :item-value="itemValue"
    item-text="name"
    :label="label || 'Category'"
    :persistent-placeholder="persistentPlaceholder"
    :required="required"
    :single-line="singleLine"
    :hide-details="hideDetails"
    :clearable="!required"
    :rules="required ? [ruleRequired] : []"
    @click:clear="input('')"
  >
  </v-select>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useCategories } from '@/composable/form';

export default defineComponent({
  name: 'CategorySelect',
  props: {
    value: String,
    itemValue: {
      type: String,
      default: 'uid',
    },
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props, { emit }) {
    const { data, loading } = useCategories();

    function input(val: string) {
      emit('input', val);
    }

    function ruleRequired(val: string): boolean | string {
      return !!val.trim() || 'Please select categories';
    }

    return {
      data,
      loading,
      input,
      ruleRequired,
    };
  },
});
</script>
