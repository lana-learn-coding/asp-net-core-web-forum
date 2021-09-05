<template>
  <v-select
    :value="value"
    @input="input"
    :items="data"
    item-value="id"
    item-text="text"
    :error-messages="errorMessages"
    :label="label || 'Thread Status'"
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
  name: 'ThreadStatusSelect',
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
      { text: 'Approved', id: 0 },
      { text: 'Pending', id: 1 },
      { text: 'Rejected', id: 2 },
      { text: 'Closed', id: 3 },
    ];

    function input(val: string) {
      emit('input', val);
    }

    function ruleRequired(val: string): boolean | string {
      return val != null || 'Please select thread status';
    }

    return {
      input,
      ruleRequired,
      data,
    };
  },
});
</script>
