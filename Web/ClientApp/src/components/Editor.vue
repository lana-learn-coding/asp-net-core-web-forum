<template>
  <ckeditor :editor="editor" v-model="input"></ckeditor>
</template>

<script lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import CKEditor from '@ckeditor/ckeditor5-vue2';
import { computed, defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'Editor',

  props: {
    value: { type: String },
    errorMessages: { type: [String, Array] },
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
