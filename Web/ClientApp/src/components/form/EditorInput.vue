<template>
  <v-input :error-messages="errorMessages" class="mt-2">
    <div :class="{ 'pt-5': !!label }" style="width: 100%">
      <label
        class="v-label theme--light"
        style="left: 0; top: 0; position: absolute; font-size: 13px">{{ label }}</label>
      <ckeditor :editor="editor" :config="config" v-model="input"></ckeditor>
    </div>
  </v-input>
</template>

<script lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';

import Essentials from '@ckeditor/ckeditor5-essentials/src/essentials';
import Autoformat from '@ckeditor/ckeditor5-autoformat/src/autoformat';
import Bold from '@ckeditor/ckeditor5-basic-styles/src/bold';
import Italic from '@ckeditor/ckeditor5-basic-styles/src/italic';
import BlockQuote from '@ckeditor/ckeditor5-block-quote/src/blockquote';
import Heading from '@ckeditor/ckeditor5-heading/src/heading';
import Image from '@ckeditor/ckeditor5-image/src/image';
import ImageCaption from '@ckeditor/ckeditor5-image/src/imagecaption';
import ImageStyle from '@ckeditor/ckeditor5-image/src/imagestyle';
import ImageResize from '@ckeditor/ckeditor5-image/src/imageresize';
import ImageToolbar from '@ckeditor/ckeditor5-image/src/imagetoolbar';
import ImageUpload from '@ckeditor/ckeditor5-image/src/imageupload';
import Indent from '@ckeditor/ckeditor5-indent/src/indent';
import Link from '@ckeditor/ckeditor5-link/src/link';
import List from '@ckeditor/ckeditor5-list/src/list';
import MediaEmbed from '@ckeditor/ckeditor5-media-embed/src/mediaembed';
import Paragraph from '@ckeditor/ckeditor5-paragraph/src/paragraph';
import PasteFromOffice from '@ckeditor/ckeditor5-paste-from-office/src/pastefromoffice';
import Table from '@ckeditor/ckeditor5-table/src/table';
import TableToolbar from '@ckeditor/ckeditor5-table/src/tabletoolbar';
import TextTransformation from '@ckeditor/ckeditor5-typing/src/texttransformation';
import SimpleUploadAdapter from '@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter';
import CKEditor from '@ckeditor/ckeditor5-vue2';
import { computed, defineComponent } from '@vue/composition-api';
import { useHttp } from '@/services/http';

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

  data() {
    return {
      editor: ClassicEditor,
    };
  },

  setup(props, { emit }) {
    const input = computed({
      get: () => props.value,
      set: (val) => emit('input', val),
    });

    const http = useHttp();
    const config = {
      plugins: [
        Essentials,
        Autoformat,
        Bold,
        Italic,
        BlockQuote,
        Heading,
        Image,
        ImageCaption,
        ImageStyle,
        ImageToolbar,
        ImageUpload,
        Indent,
        Link,
        List,
        MediaEmbed,
        Paragraph,
        PasteFromOffice,
        Table,
        TableToolbar,
        TextTransformation,
        SimpleUploadAdapter,
        ImageResize,
      ],
      simpleUpload: {
        uploadUrl: `${http.client.defaults.baseURL}/data/images`,
        headers: {
          Authorization: http.client.defaults.headers.Authorization ?? '',
        },
      },
      toolbar: {
        items: [
          'heading',
          '|',
          'bold',
          'italic',
          'link',
          'bulletedList',
          'numberedList',
          '|',
          'outdent',
          'indent',
          '|',
          'uploadImage',
          'blockQuote',
          'insertTable',
          'mediaEmbed',
          'undo',
          'redo',
        ],
      },
      image: {
        toolbar: [
          'imageStyle:inline',
          'imageStyle:block',
          'imageStyle:side',
          '|',
          'toggleImageCaption',
          'imageTextAlternative',
        ],
      },
      table: {
        contentToolbar: [
          'tableColumn',
          'tableRow',
          'mergeTableCells',
        ],
      },
      language: 'en',
    };

    return {
      input,
      config,
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
