<template>
  <v-card flat>
    <template v-if="!forum.uid">
      <v-card-text>
        <v-skeleton-loader type="heading" class="mb-2"></v-skeleton-loader>
        <v-skeleton-loader type="paragraph"></v-skeleton-loader>
      </v-card-text>
    </template>
    <template v-else>
      <v-card-title>{{ forum.title }}</v-card-title>
      <v-card-subtitle class="pb-2">{{ forum.subTitle }}</v-card-subtitle>
      <v-card-text class="pb-0">
        <div v-html="forum.description" class="ck-content"></div>
        <div class="mt-2 d-flex">
          <div>
            <div class="mr-1 d-inline">
              <v-icon small>chat_bubble_outline</v-icon>
              {{ forum.postsCount }}
            </div>

            <div class="mr-1 d-inline">
              <v-icon class="material-icons-outlined" small>question_answer</v-icon>
              {{ forum.threadsCount || 1 }}
            </div>

            <div class="mr-1 d-inline">
              <v-icon class="material-icons-outlined" small>visibility</v-icon>
              {{ forum.viewsCount || forum.postsCount * 2 }}
            </div>
          </div>

          <v-spacer></v-spacer>

          <div>
            <v-tooltip bottom v-if="forum.threadAccess === 1">
              <template v-slot:activator="{ on, attrs }">
                <v-icon small v-bind="attrs" v-on="on" class="material-icons-outlined">beenhere
                </v-icon>
              </template>
              <span>Require approval when creating thread here</span>
            </v-tooltip>

            <v-tooltip bottom v-if="forum.threadAccess > 1">
              <template v-slot:activator="{ on, attrs }">
                <v-icon small v-bind="attrs" v-on="on" class="material-icons-outlined">
                  admin_panel_settings
                </v-icon>
              </template>
              <span>Only admin can create a thread here</span>
            </v-tooltip>
          </div>
        </div>
      </v-card-text>
    </template>

    <v-card-text>
      <v-divider class="mb-3"></v-divider>
      <slot name="action"></slot>
      <v-skeleton-loader
        v-if="loading || !forum.uid"
        type="list-item-avatar-three-line@4"
      >
      </v-skeleton-loader>

      <div v-else-if="!threads.length">
        <div class="body-1 text-center pa-8">Nothing here :(</div>
      </div>
      <template v-else>
        <div v-for="thread in threads" :key="thread.uid">
          <thread-row :thread="thread"></thread-row>
        </div>
      </template>
      <slot name="footer"></slot>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Forum, Thread } from '@/services/model';
import ThreadRow from '@/components/forum/ThreadRow.vue';

export default defineComponent({
  name: 'ThreadList',
  components: { ThreadRow },
  props: {
    forum: {
      type: Object as PropType<Forum>,
    },
    threads: {
      type: Array as PropType<Thread[]>,
      default: () => [],
    },
    loading: {
      type: Boolean,
    },
  },
});
</script>
