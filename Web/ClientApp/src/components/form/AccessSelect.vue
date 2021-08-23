<template>
  <v-select
    :value="value"
    @input="input"
    :items="data"
    item-value="id"
    item-text="text"
    :error-messages="errorMessages"
    :label="label || 'Access Type'"
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

export default defineComponent({
  name: 'AccessSelect',
  props: {
    value: [Number, String],
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props, { emit }) {
    const data = [
      { text: 'Public', id: 0 },
      { text: 'Protected', id: 1 },
      { text: 'Private', id: 3 },
    ];

    function input(val: string) {
      emit('input', val);
    }

    function ruleRequired(val: string): boolean | string {
      return val != null || 'Please select access type';
    }

    return {
      input,
      ruleRequired,
      data,
    };
  },
});
</script>
