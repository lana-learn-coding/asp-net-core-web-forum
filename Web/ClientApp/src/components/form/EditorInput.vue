<template>
  <v-input :error-messages="errorMessages" class="mt-2">
    <div :class="{ 'pt-5': !!label }" style="width: 100%">
      <label
        class="v-label theme--light"
        style="left: 0; top: 0; position: absolute; font-size: 13px">{{ label }}</label>
      <ckeditor :editor="editor" v-model="input"></ckeditor>
    </div>
  </v-input>
</template>

<script lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import CKEditor from '@ckeditor/ckeditor5-vue2';
import { computed, defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'EditorInput',

  props: {
    value: { type: String },
    errorMessages: { type: [String, Array] },
    label: String,
  },

  components: {
    ckeditor: CKEditor.component,
  },

  setup(props, { emit }) {
    return {
      editor: ClassicEditor,
      input: computed({
        get: () => props.value,
        set: (val) => emit('input', val),
      }),
    };
  },
});
</script>

<style>
.ck-editor {
  width: 100% !important;
}

.ck-editor__editable {
  min-height: 300px;
  width: 100%;
}
</style>
