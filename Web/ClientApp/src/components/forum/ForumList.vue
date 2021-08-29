<template>
  <v-card flat>
    <v-hover #default="{ hover }">
      <div
        @click="$router.push({ name: 'Category', params: { slug: category.slug } })"
        v-ripple
        style="cursor: pointer"
      >
        <v-card-title :class="{ 'text--secondary': hover }">{{ category.name }}</v-card-title>
        <v-card-subtitle v-if="category.description">{{ category.description }}</v-card-subtitle>
      </div>
    </v-hover>

    <v-card-text>
      <v-skeleton-loader v-if="loading" type="list-item-avatar-three-line@5">
      </v-skeleton-loader>
      <div v-else-if="!forums.length">
        <v-divider></v-divider>
        <div class="body-1 text-center pa-8">Nothing here :(</div>
      </div>
      <div v-else v-for="forum in forums" :key="forum.uid">
        <v-divider></v-divider>
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

          <v-col>
            <router-link
              class="body-1 text-truncate text-decoration-none font-weight-medium text-link"
              :to="{ name: 'Forum', params: { slug: forum.slug }, meta: { roles: checkRole(forum) } }"
            >
              {{ forum.title }}
            </router-link>
            <div class="body-2">{{ forum.subTitle }}</div>
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
          <v-col cols="2">
            <div class="body-2">
              <number-counter from="0" :to="forum.threadCounts || 0" :duration="1"></number-counter>
              Threads
            </div>
            <div class="body-2">
              <number-counter from="0" :to="forum.postCounts || 0" :duration="1"></number-counter>
              Posts
            </div>
          </v-col>
        </v-row>
      </div>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Category, Forum } from '@/services/model';
import NumberCounter from '@/components/NumberCounter.vue';

export default defineComponent({
  name: 'ForumList',
  components: { NumberCounter },
  props: {
    category: {
      type: Object as PropType<Category>,
    },
    forums: {
      type: Array as PropType<Forum[]>,
    },
    loading: {
      type: Boolean,
    },
  },
  setup() {
    function checkRole(forum: Forum): string[] | boolean {
      if (forum.threadAccess < 1) {
        return false;
      }
      return forum.threadAccess === 1 ? ['User'] : ['Admin'];
    }

    return {
      checkRole,
    };
  },
});
</script>
