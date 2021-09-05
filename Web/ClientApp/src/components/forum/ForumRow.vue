<template>
  <v-row class="py-2 py-xl-5">
    <v-hover #default="{ hover }">
      <v-col cols="2" md="1">
        <router-link
          v-ripple
          class="d-flex justify-center align-center w-full h-full text-decoration-none"
          :to="{ name: 'Forum', params: { slug: forum.slug } }"
        >
          <v-icon
            :small="$vuetify.breakpoint.mdOnly"
            :color="hover ? 'primary' : ''"
          >
            {{ forum.icon || 'folder' }}
          </v-icon>
        </router-link>
      </v-col>
    </v-hover>

    <v-col cols="10" md="9" lg="5">
      <router-link
        class="body-1 text-decoration-none font-weight-medium text-link text-truncate d-block"
        :to="{ name: 'Forum', params: { slug: forum.slug } }"
      >
        {{ forum.title }}
      </router-link>
      <div class="body-2 text-truncate">{{ forum.subTitle }}</div>
      <span class="d-lg-none">{{ formatDateTime(forum.lastThread.lastActivityAt) }}</span>
      <div>
        <v-tooltip bottom v-if="forum.forumAccess > 0">
          <template v-slot:activator="{ on, attrs }">
            <v-icon
              small
              v-bind="attrs"
              v-on="on"
              class="material-icons-outlined"
              :color="forum.forumAccess > 1 ? 'error darken-1' : ''">
              lock
            </v-icon>
          </template>
          <span>Only member can access this forum</span>
        </v-tooltip>

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
    </v-col>
    <v-col class="d-none d-lg-block" md="4">
      <div v-if="forum.lastThread">
        <v-row>
          <v-col cols="3" class="d-flex flex-column align-center justify-center">
            <div class="text-truncate caption font-weight-medium">Last Activity</div>
            <v-avatar size="38">
              <v-img
                :src="forum.lastThread.user.avatar || '@/assets/anon.png'"
                lazy-src="@/assets/anon_thumbnail.png"
                :alt="forum.lastThread.user.username"
              ></v-img>
            </v-avatar>
            <div class="text-truncate caption">{{ forum.lastThread.user.username }}</div>
          </v-col>
          <v-col cols="9">
            <router-link
              class="body-2 text-decoration-none font-weight-medium text-truncate d-block text--secondary"
              :to="{ name: 'Thread', params: { slug: forum.lastThread.slug } }"
            >
              {{ forum.lastThread.title }}
            </router-link>
            <span>{{ formatDateTime(forum.lastThread.lastActivityAt) }}</span>
          </v-col>
        </v-row>
      </div>
    </v-col>
    <v-col md="2">
      <span class="body-2 d-md-block mr-3">
        <number-counter from="0" :to="forum.threadsCount || 0" :duration="1"></number-counter>
        Threads
      </span>
      <span class="body-2 d-md-block mr-3">
        <number-counter from="0" :to="forum.postsCount || 0" :duration="1"></number-counter>
        Posts
      </span>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import NumberCounter from '@/components/NumberCounter.vue';
import { Forum } from '@/services/model';
import { formatDateTime } from '@/composable/date';

export default defineComponent({
  name: 'ForumRow',
  components: { NumberCounter },
  props: {
    forum: Object as PropType<Forum>,
  },
  setup() {
    return {
      formatDateTime,
    };
  },
});
</script>
