<template>
  <v-select
    :value="value"
    @input="input"
    :items="['Public', 'Protected', 'Private']"
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
    value: String,
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props, { emit }) {
    function input(val: string) {
      emit('input', val);
    }

    function ruleRequired(val: string): boolean | string {
      return !!val.trim() || 'Please select categories';
    }

    return {
      input,
      ruleRequired,
    };
  },
});
</script>
